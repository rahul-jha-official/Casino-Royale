<template>
  <Teleport to="#portal">
    <div class="portal-container">
      <div
        class="portal-underlay"
        role="button"
        tabindex="0"
        @click="handleCloseModal"
      >
        <div class="portal-content">
          <slot></slot>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<script setup></script>

<style scoped>

.portal-container {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  z-index: 2000;
  pointer-events: all;
}

.portal-underlay {
  position: absolute;
  inset: 0;
  background: rgba(24, 27, 36, 0.82);
  z-index: 1999;
  border: none;
  padding: 0;
  width: 100vw;
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
}

.portal-content {
  position: relative;
  z-index: 2001;
  width: 420px;
  max-width: 92vw;
  max-height: 84vh;
  background: #23283a;
  border-radius: 16px;
  padding: 2rem 1.5rem 1.5rem 1.5rem;
  border: 1.5px solid #181b24;
  box-shadow: 0 8px 32px rgba(0,0,0,0.28);
  display: flex;
  flex-direction: column;
  align-items: center;
  overflow-y: auto;
  color: #f3f3f3;
  animation: portal-fade-in 0.18s cubic-bezier(.4,0,.2,1);
}

@keyframes portal-fade-in {
  from { opacity: 0; transform: scale(0.98); }
  to { opacity: 1; transform: scale(1); }
}
</style>
