import { useAuthStore } from "@/stores/authStores";

export const requireAuth = () => {
    const authStore = useAuthStore();
    if (!authStore.isAuthenticated) {
        // Redirect to login page if not authenticated
        return { name: 'Login' };
    }
    return true; // User is authenticated, allow access
};

export const notRequireAuth = () => {
    const authStore = useAuthStore();
    if (authStore.isAuthenticated) {
        // Redirect to home page if authenticated
        return { name: 'Home' };
    }
    return true; // User is authenticated, allow access
};