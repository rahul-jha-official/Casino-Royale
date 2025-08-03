import { defineStore } from "pinia";
import { reactive, ref, computed } from "vue";
import authService from "@/services/authService";
import router from "@/components/router/routes";
import Cookie from "js-cookie";

export const useAuthStore = defineStore("authStore", () => {
  // state
  const user = reactive({
    email: "",
    name: "",
    id: "",
    token: "",
    role: "",
  });

  const isAuthenticated = ref(false);

  const getUserInfo = computed(() => {
    return isAuthenticated.value ? user : null;
  });

  function initialize() {
    try {
      const token = Cookie.get("token_casino");
      if (token) {
        const payload = JSON.parse(atob(token.split(".")[1]));
        const ud = {
          email: payload.email,
          role: payload.role,
          name: payload.fullname,
          id: payload.id,
          token: token,
        };
        Object.assign(user, ud);
        isAuthenticated.value = true;
      } else {
        clear();
      }
    } catch (error) {
      console.log("Error Initializing");
    }
  }

  function signOut() {
    clear();
  }

  function clear() {
    Object.assign(user, {
      email: "",
      name: "",
      role: "",
      id: "",
      token: "",
    });
    isAuthenticated.value = false;
    Cookie.remove("token_casino");
  }

  async function signup(data) {
    try {
      await authService.signup(data);
      router.push("/login");
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.errorMessage ?? error.message,
      };
    }
  }

  async function signin(data) {
    try {
      const { token, user: ud } = await authService.login(data);
      if (!token) {
        return {
          success: false,
          message: "Login Failed",
        };
      }
      Object.assign(user, ud);
      user.token = token;
      isAuthenticated.value = true;
      Cookie.set("token_casino", token, { expires: 1 });
      router.push("/");
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.errorMessage ?? error.message,
      };
    }
  }

  return {
    user,
    isAuthenticated,
    getUserInfo,
    signup,
    signin,
    signOut,
    initialize,
  };
});
