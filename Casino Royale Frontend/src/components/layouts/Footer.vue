<template>
  <footer>
    <a href="https://github.com/rahul-jha-official" target="_blank">
      <img
        alt="pfp"
        src="https://github.com/rahul-jha-official/rahul-jha-official/assets/138975150/de7348d4-4b31-4537-a8e4-eebfa88eac8f"
      />
      <p>rahul-jha-official</p>
      <i class="fa-brands fa-github"></i>
    </a>
  </footer>
</template>

<style scoped>
footer {
  width: 100vw;
  background: #222;
  color: #fff;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  align-items: center;
  padding: 0.8rem 0 0.8rem 0;
  margin: 0 auto;
  box-shadow: 0 -2px 8px rgba(0, 0, 0, 0.07);
  z-index: 999;
  position: fixed;
  left: 0;
  bottom: 0;
}

footer a {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.3rem 0.8rem 0.3rem 0.3rem;
  background: #fff;
  color: #222;
  border-radius: 4rem;
  border: 1px solid transparent;
  transition: border 0.2s, background 0.2s, color 0.2s;
  text-decoration: none;
  font-weight: 500;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.04);
}

footer a:hover {
  border-color: #007bff;
  background: #f2f2f2;
  color: #007bff;
}

footer a img {
  max-width: 20px;
  aspect-ratio: 1 / 1;
  border-radius: 100%;
  border: 2px solid #007bff;
  background: #fff;
}

footer p {
  margin: 0;
  font-size: 0.8rem;
  font-weight: 500;
}
</style>
