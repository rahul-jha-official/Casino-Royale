<template>
  <div class="auth-bg">
    <section class="signup-container">
      <h1>Sign Up</h1>
      <form class="signup-form" @submit.prevent>
        <div class="form-group">
          <label for="name">Name</label>
          <input
            type="text"
            id="name"
            v-model="form.name"
            placeholder="Enter your name"
            required
          />
        </div>
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
        <div class="form-group">
          <label for="confirmPassword">Confirm Password</label>
          <input
            type="password"
            id="confirmPassword"
            v-model="form.confirmPassword"
            placeholder="Confirm your password"
            required
          />
        </div>
        <div class="alert alert-danger" v-if="error.length > 0">
          <span class="d-block" v-for="e in error"> {{ e }}</span>
        </div>
        <button @click="handleSignup" class="signup-btn">
          <span
            class="spinner-border spinner-border-sm me-2"
            v-if="isLoading"
          ></span>
          Sign Up
        </button>
      </form>
      <div class="login-link">
        <span>Already have an account?</span>
        <router-link :to="{ name: 'Login' }">Login</router-link>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, reactive } from "vue";
import { useAuthStore } from "@/stores/authStores";

const authStore = useAuthStore();

const form = reactive({
  name: "",
  email: "",
  password: "",
  confirmPassword: "",
});

const isLoading = ref(false);
const error = ref([]);

async function handleSignup() {
  error.value = [];
  isLoading.value = true;
  if (form.name === undefined || form.name.length < 2) {
    error.value.push("Name must be at least 2 characters long!");
  }
  if (
    form.email === undefined ||
    !form.email.length === 0 ||
    !form.email.includes("@")
  ) {
    error.value.push("Please enter a valid email address!");
  }
  if (form.password === undefined) {
    error.value.push("Please enter a valid password!");
  }
  if (form.password !== form.confirmPassword) {
    error.value.push("Passwords do not match!");
  }

  const response = await authStore.signup({
    name: form.name,
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
.signup-container {
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
.signup-container h1 {
  margin-bottom: 24px;
  font-size: 2rem;
  color: #fff;
  letter-spacing: 1px;
}
.signup-form {
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
.signup-btn {
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
.signup-btn:hover {
  background: linear-gradient(90deg, #0056b3 60%, #007bff 100%);
}
.login-link {
  margin-top: 18px;
  font-size: 0.98rem;
  color: #bfc7d5;
  text-align: center;
}
.login-link a {
  color: #4eaaff;
  text-decoration: none;
  margin-left: 6px;
  font-weight: 500;
  transition: color 0.2s;
}
.login-link a:hover {
  text-decoration: underline;
  color: #007bff;
}
</style>
