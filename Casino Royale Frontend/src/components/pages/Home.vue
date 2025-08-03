<template>
  <div class="home-bg">
    <section class="home-container">
      <h1 class="app-title">Casino Royale</h1>
      <p class="app-desc">
        Welcome to Casino Royale! Play, create, or join a room and enjoy the
        thrill of online casino games with friends.
      </p>
      <div class="home-actions">
        <button class="home-btn create" @click="openModal('CreateRoom')">
          Create Room
        </button>
        <button class="home-btn join" @click="openModal('JoinRoom')">
          Join Room
        </button>
      </div>
    </section>
  </div>
</template>

<script setup>
import { inject } from "vue";
import { useAuthStore } from "@/stores/authStores";
import { useSwal } from "@/composables/swal";

const authStore = useAuthStore();
const swal = useSwal();
const currentModal = inject("currentModal");

function openModal(modalName) {
  if (!authStore.isAuthenticated) {
    swal.showError("Please Login First");
    return;
  }
  currentModal.value = modalName;
}
</script>

<style scoped>
.home-bg {
  min-height: 100vh;
  width: 100vw;
  background: linear-gradient(135deg, #23272f 0%, #2d3240 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding-top: 64px;
  padding-bottom: var(--footer-height, 54px);
}
.home-container {
  background: #292e3a;
  border-radius: 16px;
  box-shadow: 0 2px 24px rgba(0, 0, 0, 0.18);
  padding: 48px 36px 40px 36px;
  display: flex;
  flex-direction: column;
  align-items: center;
  max-width: 420px;
  width: 100%;
  border: 1.5px solid #23272f;
}
.app-title {
  color: #fff;
  font-size: 2.4rem;
  font-weight: bold;
  margin-bottom: 18px;
  letter-spacing: 1.5px;
  text-align: center;
}
.app-desc {
  color: #bfc7d5;
  font-size: 1.1rem;
  margin-bottom: 32px;
  text-align: center;
}
.home-actions {
  display: flex;
  gap: 22px;
  width: 100%;
  justify-content: center;
}
.home-btn {
  padding: 12px 28px;
  font-size: 1.08rem;
  font-weight: 600;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  background: linear-gradient(90deg, #007bff 60%, #0056b3 100%);
  color: #fff;
  box-shadow: 0 1px 8px rgba(0, 0, 0, 0.1);
  transition: background 0.2s, transform 0.2s;
}
.home-btn.create:hover {
  background: linear-gradient(90deg, #0056b3 60%, #007bff 100%);
  transform: translateY(-2px) scale(1.04);
}
.home-btn.join {
  background: linear-gradient(90deg, #28a745 60%, #218838 100%);
}
.home-btn.join:hover {
  background: linear-gradient(90deg, #218838 60%, #28a745 100%);
  transform: translateY(-2px) scale(1.04);
}
</style>
