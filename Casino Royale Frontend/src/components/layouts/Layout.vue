<template>
  <div class="layout-root">
    <Header />
    <main class="layout-main">
      <slot />
    </main>
    <Footer />
  </div>
</template>

<script setup>
import Header from './Header.vue';
import Footer from './Footer.vue';
</script>

<style scoped>
.layout-root {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}
.layout-main {
  flex: 1 0 auto;
  display: flex;
  flex-direction: column;
}
</style>