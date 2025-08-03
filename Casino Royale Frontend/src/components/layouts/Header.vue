
<template>
  <header class="main-header">
    <nav class="nav-bar">
      <div class="nav-left">
        <router-link :to="{ name: 'Home' }" class="nav-logo" @click="closeMenu()">Casino Royale</router-link>
        <router-link :to="{ name: 'Home' }" class="nav-link" @click="closeMenu()">Home</router-link>
        <router-link :to="{ name: 'About' }" class="nav-link" @click="closeMenu()">About</router-link>
      </div>
      <div class="nav-right">
        <div v-if="authStore.isAuthenticated" class="user-info">
          <span class="user-avatar">
            <svg width="28" height="28" viewBox="0 0 28 28" fill="none">
              <circle cx="14" cy="14" r="13" fill="#23283a" stroke="#4eaaff" stroke-width="2"/>
              <text x="14" y="18" text-anchor="middle" font-size="14" fill="#4eaaff" font-family="monospace">{{ authStore.user?.name?.charAt(0) || '?' }}</text>
            </svg>
          </span>
          <span class="user-name">{{ authStore.user?.name || 'User' }}</span>
        </div>
        <div class="hamburger-menu" @click="toggleMenu">
          <span></span>
          <span></span>
          <span></span>
        </div>
        <div v-if="menuOpen" class="dropdown-menu">
          <a @click="openModal('CreateRoom')" class="dropdown-item">Create Room</a>
          <a @click="openModal('JoinRoom')" class="dropdown-item">Join Room</a>
          <a class="dropdown-item" @click="handleAuth">{{ authStore.isAuthenticated ? 'SignOut' : 'Login' }}</a>
        </div>
      </div>
    </nav>
  </header>
</template>

<script setup>
import { ref, inject } from 'vue';
import { useAuthStore } from '@/stores/authStores';
import router from '../router/routes';
import { useSwal } from '@/composables/swal';

const authStore = useAuthStore();
const swal = useSwal();
const menuOpen = ref(false);
const currentModal = inject('currentModal');

function toggleMenu() {
  menuOpen.value = !menuOpen.value;
}

function openModal(modalName) {
  if (!authStore.isAuthenticated){
    swal.showError("Please Login First");
    return;
  }
  currentModal.value = modalName;
  menuOpen.value = false
}

function closeMenu() {
  menuOpen.value = false;
}

function handleAuth() {
  closeMenu();
  if (authStore.isAuthenticated) {
    authStore.signOut();
  }
  else {
    router.push('/login')
  }
}

</script>

<style scoped>
.main-header {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  background: #222;
  z-index: 1000;
  box-shadow: 0 2px 8px rgba(0,0,0,0.07);
}
.nav-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0 32px;
  height: 60px;
  box-sizing: border-box;
}
.nav-left {
  display: flex;
  align-items: center;
  gap: 18px;
}
.nav-logo {
  font-size: 1.3rem;
  font-weight: bold;
  color: #fff;
  text-decoration: none;
  margin-right: 18px;
}
.nav-link {
  color: #fff;
  text-decoration: none;
  font-size: 1rem;
  padding: 6px 12px;
  border-radius: 4px;
  transition: background 0.2s;
}
.nav-link.active, .nav-logo.active {
  background: #007bff;
  color: #fff;
  box-shadow: 0 2px 8px #007bff44;  
}
.nav-link:hover {
  background: #444;
}
.nav-right {
  position: relative;
  display: flex;
  align-items: center;
  gap: 18px;
}
.user-info {
  display: flex;
  align-items: center;
  gap: 8px;
  background: linear-gradient(90deg, #23283a 60%, #181b24 100%);
  border-radius: 18px;
  padding: 4px 14px 4px 8px;
  box-shadow: 0 2px 8px #4eaaff22;
  border: 1.5px solid #23283a;
  animation: glowUser 2.2s ease-in-out infinite alternate;
}
.user-avatar {
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 2px;
}
.user-name {
  color: #4eaaff;
  font-weight: 600;
  font-size: 1.08rem;
  letter-spacing: 0.5px;
  text-shadow: 0 0 8px #4eaaff88;
}
@keyframes glowUser {
  0% { box-shadow: 0 2px 8px #4eaaff22; }
  100% { box-shadow: 0 2px 16px #4eaaff66; }
}
.hamburger-menu {
  width: 36px;
  height: 36px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  cursor: pointer;
  gap: 5px;
}
.hamburger-menu span {
  display: block;
  width: 26px;
  height: 4px;
  background: #fff;
  border-radius: 2px;
  transition: 0.2s;
}
.dropdown-menu {
  position: absolute;
  top: 48px;
  right: 0;
  background: #23283a;
  box-shadow: 0 2px 12px rgba(0,0,0,0.18);
  border-radius: 10px;
  min-width: 170px;
  display: flex;
  flex-direction: column;
  padding: 10px 0;
  z-index: 200;
  border: 1.5px solid #181b24;
}
.dropdown-item {
  color: #f3f3f3;
  text-decoration: none;
  padding: 12px 20px;
  font-size: 1rem;
  transition: background 0.18s, color 0.18s;
  cursor: pointer;
  border-radius: 6px;
}
.dropdown-item.active {
  background: #007bff;
  color: #fff;
  box-shadow: 0 2px 8px #007bff44;
}
.dropdown-item:hover {
  background: #31374a;
  color: #4eaaff;
}
</style>
