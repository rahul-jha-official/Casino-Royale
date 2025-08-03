
<template>
  <div class="room-detail-modal">
    <h2>Room Details</h2>
    <div class="room-info-row">
      <span class="room-label">Room ID:</span>
      <span class="room-id">{{ casinoStore.roomDetails.roomId }}</span>
      <button class="copy-btn" @click="copyRoomId" title="Copy Room ID">
        <i class="fa-regular fa-copy"></i>
      </button>
    </div>
    <div class="room-info-row">
      <span class="room-label">Max People:</span>
      <span class="room-value">
        {{ casinoStore.roomDetails.maxPeople }}
      </span>
    </div>
    <button class="join-btn" @click="handleJoinRoom()">Join Room</button>
  </div>
</template>

<script setup>
import { inject } from 'vue';
import router from '@/components/router/routes';
import { useCasinoStore } from '@/stores/casinoStore';
const casinoStore = useCasinoStore();
const currentModal = inject('currentModal');

function copyRoomId() {
  navigator.clipboard.writeText(useCasinoStore.roomDetails.roomId);
}

function handleJoinRoom() {
  router.push({ path: `/room/${casinoStore.roomDetails.roomId}` })
  currentModal.value = null; // Close the modal after navigating
}
</script>

<style scoped>
.room-detail-modal {
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
}
.room-detail-modal h2 {
  color: #fff;
  font-size: 1.5rem;
  margin-bottom: 18px;
  letter-spacing: 1px;
}
.room-info-row {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 18px;
  width: 100%;
  justify-content: center;
}
.room-label {
  color: #bfc7d5;
  font-weight: 500;
  font-size: 1rem;
}
.room-id {
  color: #4eaaff;
  font-size: 1.08rem;
  font-family: 'Fira Mono', 'Consolas', monospace;
  background: #23272f;
  padding: 4px 18px;
  border-radius: 6px;
  border: 1px solid #444a5a;
  min-width: 120px;
  text-align: center;
  letter-spacing: 1px;
  user-select: all;
}
.room-value {
  color: #4eaaff;
  font-size: 1.08rem;
  font-family: 'Fira Mono', 'Consolas', monospace;
  background: #23272f;
  padding: 4px 10px;
  border-radius: 6px;
  border: 1px solid #444a5a;
}
.copy-btn {
  background: #292e3a;
  color: #4eaaff;
  border: 1px solid #007bff;
  border-radius: 6px;
  padding: 6px 12px;
  font-size: 1.1rem;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.18s, color 0.18s;
  display: flex;
  align-items: center;
  justify-content: center;
}
.copy-btn i {
  font-size: 1.2rem;
}
.copy-btn:hover {
  background: #007bff;
  color: #fff;
}
.join-btn {
  margin-top: 10px;
  padding: 10px 0;
  width: 100%;
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
</style>
