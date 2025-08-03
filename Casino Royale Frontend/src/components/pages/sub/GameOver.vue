<template>
  <div class="gameover-modal">
    <h2 class="gameover-title">Game Over</h2>
    <div class="result-info">
      <div class="result-row">
        <h2 v-if="isWinner" class="winner-text">🎉 You Won! 🎉</h2>
        <h2 v-else class="loser-text">😢 You Lost 😢</h2>
      </div>
    </div>
    <button class="declare-btn" @click="goHome">Go to Home</button>
  </div>
</template>

<script setup>
import { defineEmits } from 'vue';

defineProps({
    isWinner: Boolean
})
const emit = defineEmits(['close-popup']);
function goHome() {
  emit('close-popup');
}
</script>
<style scoped>
.gameover-modal {
  width: 100%;
  max-width: 480px;
  margin: 0 auto;
  background: linear-gradient(135deg, #23283a 0%, #2d3240 100%);
  border-radius: 18px;
  box-shadow: 0 2px 18px #007bff22, 0 1.5px 8px #4eaaff22;
  padding: 32px 24px 24px 24px;
  display: flex;
  flex-direction: column;
  align-items: center;
}
.result-row {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
  position: relative;
}
.gameover-modal h2 {
  color: #4eaaff;
  font-size: 2rem;
  margin-bottom: 24px;
  text-shadow: 0 2px 8px #4eaaff44;
}
.winner-list {
  list-style: none;
  padding: 0;
  margin: 0 0 24px 0;
  width: 100%;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 18px;
}
.winner-item {
  display: flex;
  align-items: center;
  gap: 10px;
  background: #292e3a;
  border-radius: 10px;
  padding: 12px 10px;
  box-shadow: 0 1px 6px rgba(0,0,0,0.08);
  border: 2px solid #23272f;
  cursor: pointer;
  position: relative;
  transition: background 0.2s, border 0.2s;
}
.winner-item.selected {
  border-color: #ffd700;
  background: #23283a;
  box-shadow: 0 0 12px #ffd70088;
}
.winner-item.offline {
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
.score, .locked {
  color: #bfc7d5;
  font-size: 0.93rem;
}
.winner-crown {
  font-size: 2rem;
  color: #ffd700;
  filter: drop-shadow(0 0 6px #ffd70088);
  margin-left: 8px;
}
.loser-icon {
  font-size: 2rem;
  color: #ff4e4e;
  filter: drop-shadow(0 0 6px #ff4e4e88);
  margin-left: 8px;
}
.declare-btn {
  margin-top: 10px;
  padding: 12px 32px;
  background: linear-gradient(90deg, #ffd700 60%, #ffb700 100%);
  color: #23283a;
  border: none;
  border-radius: 8px;
  font-size: 1.1rem;
  font-weight: 700;
  cursor: pointer;
  box-shadow: 0 1px 8px #ffd70044;
  transition: background 0.2s;
}
.declare-btn:disabled {
  background: #888;
  color: #fff;
  cursor: not-allowed;
}
</style>
