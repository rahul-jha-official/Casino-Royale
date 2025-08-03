<template>
  <section v-if="gameOver">
    <Portal>
      <div class="centered-popup">
        <component
          :is="gameOverPopup"
          :is-winner="isWinner"
          @close-popup="closePopup"
        />
      </div>
    </Portal>
  </section>
  <section v-else>
    <div class="room-layout">
      <aside class="room-people">
        <ul>
          <li
            v-for="person in people"
            :key="person.id"
            :class="{ offline: !person.online }"
          >
            <div class="avatar-wrapper">
              <img :src="person.img" :alt="person.name" class="avatar" />
              <span
                class="dot-avatar"
                :class="person.online ? 'online' : 'offline'"
              ></span>
            </div>
            <div class="person-info">
              <span class="name">{{ person.name }}</span>
              <span class="score"
                >Balance: <b>{{ person.score }}</b></span
              >
              <span class="locked"
                >Locked: <b>{{ person.locked }}</b></span
              >
            </div>
          </li>
        </ul>
      </aside>

      <section class="room-dice">
        <div class="dice-row">
          <Dice :spinDice="dice1" />
          <Dice :spinDice="dice2" class="dice-padding-left" />
        </div>
        <div class="round-title">Round {{ currentRound }}</div>
      </section>

      <aside class="room-lock">
        <h3>Lock Your Value</h3>
        <div class="radio-group">
          <label class="custom-radio">
            <input
              type="radio"
              v-model="bidOperationType"
              name="bidOperationType"
              value="0"
              :disabled="isLocked"
            />
            <span class="radio-mark"></span>
            <span class="radio-label">&lt; 7</span>
          </label>
          <label class="custom-radio">
            <input
              type="radio"
              v-model="bidOperationType"
              name="bidOperationType"
              value="1"
              :disabled="isLocked"
            />
            <span class="radio-mark"></span>
            <span class="radio-label">= 7</span>
          </label>
          <label class="custom-radio">
            <input
              type="radio"
              v-model="bidOperationType"
              name="bidOperationType"
              value="2"
              :disabled="isLocked"
            />
            <span class="radio-mark"></span>
            <span class="radio-label">&gt; 7</span>
          </label>
        </div>
        <div class="lock-form">
          <input
            type="number"
            min="1"
            max="12"
            placeholder="Enter value"
            v-model="lockedValue"
            :disabled="isLocked"
          />
          <button @click="handleLock" :disabled="isLocked">Lock</button>
        </div>
      </aside>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, watch } from "vue";
import Dice from "../widgets/Dice.vue";
import connection from "@/clients/signalr";
import router from "../router/routes";
import { useCasinoStore } from "@/stores/casinoStore";
import { useAuthStore } from "@/stores/authStores";
import { useSwal } from "@/composables/swal";
import { onBeforeRouteLeave } from "vue-router";
import GameOver from "./sub/GameOver.vue";

const casinoStore = useCasinoStore();
const authStore = useAuthStore();
const swal = useSwal();
const currentRound = ref(1);
const lockedValue = ref(0);
const bidOperationType = ref("");
const isLocked = ref(false);
const people = ref([]);

const dice1 = ref(1);
const dice2 = ref(1);

const gameOver = ref(false);
const gameOverPopup = ref(null);
const isWinner = ref(null);
const isGameFinished = ref(false);

function closePopup() {
  gameOverPopup.value = null;
  isGameFinished.value = true;
  router.push({ name: "Home" });
}

connection.on("gameOver", (players) => {
  var maxPoint = 0;
  for (let i = 0; i < players.length; i++) {
    maxPoint = Math.max(maxPoint, players[i].currentBalance);
  }

  isWinner.value = false;

  for (let i = 0; i < players.length; i++) {
    if (
      players[i].currentBalance == maxPoint &&
      players[i].id === authStore.user.id
    ) {
      isWinner.value = true;
    }
  }

  gameOverPopup.value = GameOver;
  gameOver.value = true;
});

if (casinoStore.roomDetails.roomId === "") {
  router.push({ name: "Home" });
}

onMounted(() => {
  fetchPlayers();
  fetchCurrentRound();
});

connection.on("setCurrentRound", (round) => {
  currentRound.value = round;
});

function fetchCurrentRound() {
  connection
    .invoke("GetCurrentRound", casinoStore.roomDetails.roomId)
    .then((round) => (currentRound.value = round));
}

onBeforeRouteLeave(async (to, from, next) => {
  if (isGameFinished.value) {
    next(true);
  } else {
    var title = "Leaving Game Already ?";
    var message = "By clicking Sure you will be out of the game 🥹";
    if ((await swal.showConfirm(title, message)).isConfirmed) {
      //Logic to leave room
      connection.send("LeaveRoom", casinoStore.roomDetails.roomId);
      next(true);
    } else {
      next(false);
    }
  }
});

function fetchPlayers() {
  connection
    .invoke("GetPlayersByRoomId", casinoStore.roomDetails.roomId)
    .then((players) => {
      if (players && players.length > 0) {
        console.log(players);
        people.value = players.map((player, index) => ({
          id: player.id,
          name: player.name,
          email: player.email,
          img: `https://randomuser.me/api/portraits/lego/${index}.jpg`,
          score: player.currentBalance,
          locked: player.currentBet,
        }));
      }
    })
    .catch((err) => {
      console.error("Error fetching players:", err);
    });
}

connection.on("fetchPlayer", () => {
  fetchPlayers();
});

connection.on("rollDice", (d1, d2) => {
  dice1.value = d1;
  dice2.value = d2;
});

connection.on("lockBidding", () => {
  isLocked.value = true;
});

connection.on("newPersonJoined", (name, email) => {
  swal.showInfo(`${name} [${email}] Joined.`);
});

connection.on("personLeft", (name, email) => {
  swal.showInfo(`${name} [${email}] Left.`);
});

connection.on("unlockBidding", () => {
  isLocked.value = false;
  cuurentRound.value += 1;
});

function handleLock() {
  connection
    .invoke(
      "LockYourBidding",
      casinoStore.roomDetails.roomId,
      authStore.user.id,
      authStore.user.email,
      lockedValue.value,
      parseInt(bidOperationType.value)
    )
    .then((response) => {
      if (!response.isSuccess) {
        alert("Invalid Operation");
      }
    });
}
</script>

<style scoped>
section {
  background: linear-gradient(135deg, #23272f 0%, #2d3240 100%);
}

.room-layout {
  display: flex;
  height: calc(110vh - 60px - 54px); /* header/footer height */
  min-height: 530px;
}
.room-people,
.room-dice,
.room-lock {
  flex: 1 1 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 24px 12px;
  box-sizing: border-box;
  margin-top: 10px;
}
.room-people {
  background: #23283a;
  border-right: 2px solid #23272f;
  min-width: 220px;
}
.room-people ul {
  list-style: none;
  padding: 0;
  margin: 0;
  width: 100%;
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 14px;
  max-height: 70vh;
  overflow-y: auto;
  scrollbar-width: thin;
  scrollbar-color: #444a5a #23283a;
}
.room-people ul::-webkit-scrollbar {
  width: 7px;
}
.room-people ul::-webkit-scrollbar-thumb {
  background: #444a5a;
  border-radius: 6px;
}
.room-people ul::-webkit-scrollbar-track {
  background: #23283a;
}
.room-people li {
  display: flex;
  align-items: center;
  gap: 10px;
  background: #292e3a;
  border-radius: 10px;
  margin: 0;
  padding: 10px 8px;
  box-shadow: 0 1px 6px rgba(0, 0, 0, 0.08);
  border: 1.5px solid #23272f;
  transition: background 0.2s;
  min-width: 0;
  max-width: 210px;
}
.room-people li.offline {
  opacity: 0.6;
}
.avatar-wrapper {
  position: relative;
  width: 38px;
  height: 38px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.avatar {
  width: 38px;
  height: 38px;
  border-radius: 50%;
  object-fit: cover;
  border: 2px solid #007bff;
}
.dot-avatar {
  position: absolute;
  bottom: 2px;
  right: 2px;
  width: 11px;
  height: 11px;
  border-radius: 50%;
  border: 2px solid #23283a;
  background: #28a745;
  box-shadow: 0 0 2px #0002;
}
.dot-avatar.offline {
  background: #888;
}
.person-info {
  display: flex;
  flex-direction: column;
  gap: 2px;
  min-width: 0;
}
.name {
  color: #fff;
  font-weight: 600;
  font-size: 1.02rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
.score,
.locked {
  color: #bfc7d5;
  font-size: 0.93rem;
}
.room-dice {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}
.dice-row {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
}
.dice-padding-left {
  padding-left: 40px;
}

.round-title {
  margin-top: 28px;
  font-size: 1.6rem;
  font-weight: 700;
  letter-spacing: 2px;
  text-align: center;
  color: #23283a;
  background: #f7fbff;
  border-radius: 14px;
  padding: 10px 32px;
  display: inline-block;
  box-shadow: 0 2px 18px #007bff22, 0 1.5px 8px #4eaaff22;
  border: 2.5px solid #4eaaff;
  animation: round-title-pop 0.7s cubic-bezier(0.4, 0, 0.2, 1);
  position: relative;
  z-index: 1;
  text-shadow: 0 2px 8px #fff8, 0 1px 0 #fff, 0 0 8px #4eaaff22;
}

@keyframes round-title-pop {
  0% {
    opacity: 0;
    transform: scale(0.8) translateY(20px);
  }
  80% {
    opacity: 1;
    transform: scale(1.08) translateY(-4px);
  }
  100% {
    opacity: 1;
    transform: scale(1) translateY(0);
  }
}

.room-lock {
  background: #23283a;
  border-left: 2px solid #23272f;
  min-width: 220px;
}
.room-lock h3 {
  color: #fff;
  margin-bottom: 18px;
  font-size: 1.2rem;
}
.radio-group {
  display: flex;
  gap: 18px;
  margin-bottom: 18px;
  justify-content: center;
}
.custom-radio {
  display: flex;
  align-items: center;
  cursor: pointer;
  position: relative;
  font-size: 1.08rem;
  font-weight: 600;
  color: #4eaaff;
  padding: 2px 10px;
  border-radius: 8px;
  transition: background 0.18s;
}
.custom-radio input[type="radio"] {
  opacity: 0;
  position: absolute;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  cursor: pointer;
}
.radio-mark {
  width: 22px;
  height: 22px;
  border-radius: 50%;
  border: 2.5px solid #4eaaff;
  background: #23283a;
  margin-right: 8px;
  box-shadow: 0 1px 6px #4eaaff22;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: border 0.18s, box-shadow 0.18s;
}
.custom-radio input[type="radio"]:checked + .radio-mark {
  border-color: #007bff;
  box-shadow: 0 0 8px #007bff88;
}
.custom-radio input[type="radio"]:checked + .radio-mark::after {
  content: "";
  display: block;
  width: 10px;
  height: 10px;
  border-radius: 50%;
  background: #007bff;
  box-shadow: 0 0 6px #007bff88;
}
.radio-label {
  color: #4eaaff;
  font-weight: 600;
  font-size: 1.08rem;
  letter-spacing: 0.5px;
  text-shadow: 0 0 8px #4eaaff44;
}
.custom-radio:hover {
  background: #23272f;
}
.lock-form {
  display: flex;
  flex-direction: column;
  gap: 12px;
  align-items: center;
}
.lock-form input[type="number"] {
  padding: 10px 14px;
  border-radius: 6px;
  border: 1.5px solid #444a5a;
  background: #23272f;
  color: #fff;
  font-size: 1rem;
  width: 150px;
  outline: none;
  margin-bottom: 6px;
}
.lock-form button {
  padding: 10px 24px;
  border-radius: 6px;
  background: linear-gradient(90deg, #007bff 60%, #0056b3 100%);
  color: #fff;
  font-weight: 600;
  border: none;
  font-size: 1rem;
  cursor: pointer;
  transition: background 0.2s;
}
.lock-form button:hover {
  background: linear-gradient(90deg, #0056b3 60%, #007bff 100%);
}
/* Center the Portal popup */
.centered-popup {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
  background: linear-gradient(135deg, #23272f 0%, #2d3240 100%);
}
</style>
