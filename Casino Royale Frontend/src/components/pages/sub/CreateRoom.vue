<template>
  <div class="create-room-modal">
    <button class="close-btn" @click="currentModal = null" aria-label="Close">
      <svg
        width="22"
        height="22"
        viewBox="0 0 22 22"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <circle cx="11" cy="11" r="11" fill="#23283a" />
        <path
          d="M7 7L15 15M15 7L7 15"
          stroke="#bfc7d5"
          stroke-width="2.2"
          stroke-linecap="round"
        />
      </svg>
    </button>
    <h2>Create Room</h2>
    <form class="create-room-form" @submit.prevent="handleCreateRoom">
      <div class="form-group">
        <label for="roomName">Room Name</label>
        <input
          type="text"
          id="roomName"
          v-model.trim="roomName"
          placeholder="Enter room name"
          required
        />
      </div>
      <div class="form-group">
        <label for="maxPeople">Max People</label>
        <input
          type="number"
          id="maxPeople"
          v-model="maxPeople"
          min="2"
          max="20"
          placeholder="e.g. 6"
          required
        />
      </div>
      <div class="form-group">
        <label for="initialAmount">Initial Amount</label>
        <input
          type="number"
          id="initialAmount"
          v-model="initialAmount"
          min="100"
          placeholder="e.g. 1000"
          required
        />
      </div>
      <div class="form-group">
        <label for="totalRounds">Total Number of Rounds</label>
        <input
          type="number"
          id="totalRounds"
          v-model="totalRounds"
          min="2"
          placeholder="e.g. 10"
          required
        />
      </div>
      <div class="form-group">
        <label for="percentSeven">% Increase if Dice Total = 7</label>
        <input
          type="number"
          id="percentSeven"
          v-model="percentSeven"
          min="0"
          max="100"
          placeholder="e.g. 50"
          required
        />
      </div>
      <div class="form-group">
        <label for="percentOther"
          >% Increase if Dice Total &lt; or &gt; 7</label
        >
        <input
          type="number"
          id="percentOther"
          v-model="percentOther"
          min="0"
          max="100"
          placeholder="e.g. 20"
          required
        />
      </div>
      <button type="submit" class="create-btn">Create Room</button>
    </form>
  </div>
</template>

<script setup>
import { inject, ref } from "vue";
import connection from "@/clients/signalr";
import { useAuthStore } from "@/stores/authStores";
import { useCasinoStore } from "@/stores/casinoStore";

const authStore = useAuthStore();
const casinoStore = useCasinoStore();

const roomName = ref("");
const maxPeople = ref(0);
const totalRounds = ref(0);
const initialAmount = ref(1000);
const percentSeven = ref(50);
const percentOther = ref(20);
const currentModal = inject("currentModal");

function handleCreateRoom() {
  if (
    roomName.value.length > 5 &&
    maxPeople.value >= 2 &&
    maxPeople.value < 10 &&
    totalRounds.value >= 2 &&
    initialAmount.value >= 1000 &&
    percentSeven.value >= 0 &&
    percentSeven.value <= 100 &&
    percentOther.value >= 0 &&
    percentOther.value <= 100 &&
    percentOther.value < percentSeven.value
  ) {
    connection
      .invoke(
        "CreateRoom",
        roomName.value,
        maxPeople.value,
        initialAmount.value,
        totalRounds.value,
        percentSeven.value,
        percentOther.value,
        authStore.user.id,
        authStore.user.email,
        authStore.user.name
      )
      .then((response) => {
        casinoStore.setRoomDetails({
          roomId: response.roomId,
          roomName: response.roomName,
          maxPeople: response.maxPlayers,
          initialAmount: response.initialAmount,
          percentSeven: response.percentSeven,
          percentOther: response.percentOther,
        });
        currentModal.value = "RoomDetail";
      })
      .catch((err) => {
        console.error("Error creating room:", err);
        alert('Failed to create room. Please try again.');
      });
  } else {
    alert("Please enter valid values for all fields.");
  }
}
</script>

<style scoped>
.create-room-modal {
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  position: relative;
}
.create-room-modal h2 {
  color: #fff;
  font-size: 1.5rem;
  margin-bottom: 18px;
  letter-spacing: 1px;
}
.close-btn {
  position: absolute;
  right: 1px;
  background: none;
  border: none;
  cursor: pointer;
  padding: 0;
  z-index: 10;
  transition: transform 0.18s;
}
.close-btn:hover svg circle {
  fill: #31374a;
}
.close-btn:hover svg path {
  stroke: #ff4e4e;
}
.create-room-form {
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 18px;
}
.form-group {
  display: flex;
  flex-direction: column;
}
.form-group label {
  margin-bottom: 6px;
  font-weight: 500;
  color: #bfc7d5;
}
.form-group input {
  padding: 10px 12px;
  border: 1px solid #444a5a;
  background: #23272f;
  color: #f3f3f3;
  border-radius: 6px;
  font-size: 1rem;
  outline: none;
  transition: border 0.2s, background 0.2s;
}
.form-group input:focus {
  border-color: #007bff;
  background: #23283a;
}
.create-btn {
  margin-top: 10px;
  padding: 10px 0;
  background: linear-gradient(90deg, #007bff 60%, #0056b3 100%);
  color: #fff;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s;
  box-shadow: 0 1px 8px rgba(0, 0, 0, 0.1);
}
.create-btn:hover {
  background: linear-gradient(90deg, #0056b3 60%, #007bff 100%);
}
</style>
