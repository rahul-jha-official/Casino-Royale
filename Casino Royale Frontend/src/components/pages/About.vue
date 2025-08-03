<template>
  <div class="about-bg">
    <section class="about-container">
      <h1>About Casino Royale</h1>
      <p class="about-desc">
        Casino Royale is a modern online casino platform where you can create or
        join rooms and play with friends. Built for fun, security, and a
        seamless experience, it brings the thrill of classic casino games to
        your screen. Enjoy, compete, and connect!
      </p>
      <div class="about-links">
        <a
          href="https://github.com/rahul-jha-official"
          target="_blank"
          class="about-link github"
        >
          <i class="fa-brands fa-github"></i> GitHub
        </a>
        <a
          href="https://www.linkedin.com/in/rahul-jha-801baa17a"
          target="_blank"
          class="about-link linkedin"
        >
          <i class="fa-brands fa-linkedin"></i> LinkedIn
        </a>
      </div>
    </section>
  </div>
</template>

<script setup>
// No logic needed
</script>

<style scoped>
.about-bg {
  min-height: 100vh;
  width: 100vw;
  background: linear-gradient(135deg, #23272f 0%, #2d3240 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding-top: 64px;
  padding-bottom: var(--footer-height, 54px);
}
.about-container {
  background: #292e3a;
  border-radius: 16px;
  box-shadow: 0 2px 24px rgba(0, 0, 0, 0.18);
  padding: 44px 32px 36px 32px;
  display: flex;
  flex-direction: column;
  align-items: center;
  max-width: 420px;
  width: 100%;
  border: 1.5px solid #23272f;
}
.about-container h1 {
  color: #fff;
  font-size: 2rem;
  font-weight: bold;
  margin-bottom: 18px;
  letter-spacing: 1px;
  text-align: center;
}
.about-desc {
  color: #bfc7d5;
  font-size: 1.08rem;
  margin-bottom: 32px;
  text-align: center;
  line-height: 1.6;
}
.about-links {
  display: flex;
  gap: 22px;
  width: 100%;
  justify-content: center;
}
.about-link {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px 22px;
  font-size: 1.08rem;
  font-weight: 600;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  background: #fff;
  color: #23272f;
  box-shadow: 0 1px 8px rgba(0, 0, 0, 0.1);
  text-decoration: none;
  transition: background 0.2s, color 0.2s, transform 0.2s;
}
.about-link.github:hover {
  background: #23272f;
  color: #fff;
  transform: translateY(-2px) scale(1.04);
}
.about-link.linkedin {
  background: #0077b5;
  color: #fff;
}
.about-link.linkedin:hover {
  background: #005983;
  color: #fff;
  transform: translateY(-2px) scale(1.04);
}
.about-link i {
  font-size: 1.2rem;
}
</style>
