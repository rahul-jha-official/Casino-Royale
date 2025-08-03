
<template>
  <div class="join-room-modal">
    <button class="close-btn" @click="currentModal = null" aria-label="Close">
      <svg width="22" height="22" viewBox="0 0 22 22" fill="none" xmlns="http://www.w3.org/2000/svg">
        <circle cx="11" cy="11" r="11" fill="#23283a"/>
        <path d="M7 7L15 15M15 7L7 15" stroke="#bfc7d5" stroke-width="2.2" stroke-linecap="round"/>
      </svg>
    </button>
    <h2>Join Room</h2>
    <form class="join-room-form" @submit.prevent>
      <div class="form-group">
        <label for="roomId">Room ID</label>
        <input type="text" id="roomId" placeholder="Enter room ID" /> <!-- Add Require Once Implemened-->
      </div>
      <button type="submit" class="join-btn" @click="GoToRoom">Join Room</button>
    </form>
  </div>
</template>

<script setup>
import { inject } from 'vue';
import router from '@/components/router/routes';
import connection from '@/clients/signalr';
import { useAuthStore } from '@/stores/authStores';
import { useCasinoStore } from '@/stores/casinoStore';
import { useSwal } from '@/composables/swal';

const authStore = useAuthStore();
const casinoStore = useCasinoStore();
const swal = useSwal();

const currentModal = inject('currentModal');

function GoToRoom() {
  if (roomId.value) {

    connection.invoke('JoinRoom', roomId.value, authStore.user.id, authStore.user.email, authStore.user.name)
      .then((response) => {
        if (response.isSuccess) {
          casinoStore.setRoomDetails({
            roomId: roomId.value,
            roomName: response.roomName,
            maxPeople: response.maxPlayers
          });
          router.push({ path: `/room/${roomId.value}` });
          currentModal.value = null;
        } else {
          currentModal.value = null;
          swal.showError(response.errorMessage);
        }
      })
      .catch(err => {
        currentModal.value = null;
        swal.showError('Failed to join room. Please try again.');
      });
  }
}

</script>

<style scoped>
.join-room-modal {
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  position: relative;
}
.join-room-modal h2 {
  color: #fff;
  font-size: 1.5rem;
  margin-bottom: 18px;
  letter-spacing: 1px;
}
.join-room-form {
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
  border-color: #28a745;
  background: #23283a;
}
.join-btn {
  margin-top: 10px;
  padding: 10px 0;
  background: linear-gradient(90deg, #28a745 60%, #218838 100%);
  color: #fff;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s;
  box-shadow: 0 1px 8px rgba(0,0,0,0.10);
}
.join-btn:hover {
  background: linear-gradient(90deg, #218838 60%, #28a745 100%);
}
.close-btn {
  position: absolute;
  top: 8px;
  right: 8px;
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
</style>
