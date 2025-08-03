<template>
  <div class="auth-bg">
    <section class="login-container">
      <h1>Login</h1>
      <form class="login-form" @submit.prevent>
        <div class="form-group">
          <label for="email">Email</label>
          <input
            type="email"
            id="email"
            v-model="form.email"
            placeholder="Enter your email"
            required
          />
        </div>
        <div class="form-group">
          <label for="password">Password</label>
          <input
            type="password"
            id="password"
            v-model="form.password"
            placeholder="Enter your password"
            required
          />
        </div>
        <div class="alert alert-danger" v-if="error.length > 0">
          <span class="d-block" v-for="e in error"> {{ e }}</span>
        </div>
        <button type="submit" class="login-btn" @click="handleLogin">
          <span
            class="spinner-border spinner-border-sm me-2"
            v-if="isLoading"
          ></span>
          Login
        </button>
      </form>
      <div class="signup-link">
        <span>Don't have an account?</span>
        <router-link :to="{ name: 'Signup' }">Sign up</router-link>
      </div>
    </section>
  </div>
</template>
<script setup>
import { ref, reactive } from "vue";
import { useAuthStore } from "@/stores/authStores";

const authStore = useAuthStore();

const form = reactive({
  email: "",
  password: "",
});

const isLoading = ref(false);
const error = ref([]);

async function handleLogin() {
  error.value = [];
  if (
    form.email === undefined ||
    !form.email.length === 0 ||
    !form.email.includes("@")
  ) {
    error.value.push("Please enter a valid email address!");
    return;
  }
  if (form.password === undefined) {
    error.value.push("Please enter a valid Password");
    return;
  }

  isLoading.value = true;
  const response = await authStore.signin({
    email: form.email,
    password: form.password,
  });

  if (response && !response.success) {
    error.value.push(response.message);
  }

  isLoading.value = false;
}
</script>

<style scoped>
html,
body,
.auth-bg {
  min-height: 100vh;
  height: 100%;
}
.auth-bg {
  min-height: 100vh;
  width: 100vw;
  background: linear-gradient(135deg, #23272f 0%, #2d3240 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  overflow-y: auto;
  padding-top: 64px;
  padding-bottom: var(--footer-height, 54px);
}
.login-container {
  max-width: 400px;
  width: 100%;
  margin: 0 auto;
  padding: 32px 24px 24px 24px;
  background: #292e3a;
  border-radius: 14px;
  box-shadow: 0 2px 24px rgba(0, 0, 0, 0.18);
  display: flex;
  flex-direction: column;
  align-items: center;
  color: #f3f3f3;
  border: 1.5px solid #23272f;
}
.login-container h1 {
  margin-bottom: 24px;
  font-size: 2rem;
  color: #fff;
  letter-spacing: 1px;
}
.login-form {
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
.login-btn {
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
.login-btn:hover {
  background: linear-gradient(90deg, #0056b3 60%, #007bff 100%);
}
.signup-link {
  margin-top: 18px;
  font-size: 0.98rem;
  color: #bfc7d5;
  text-align: center;
}
.signup-link a {
  color: #4eaaff;
  text-decoration: none;
  margin-left: 6px;
  font-weight: 500;
  transition: color 0.2s;
}
.signup-link a:hover {
  text-decoration: underline;
  color: #007bff;
}
</style>
