import { createApp } from 'vue'
import App from './App.vue'
import router from './components/router/routes.js'
import { createPinia } from "pinia";
import { useAuthStore } from './stores/authStores';
import { useCasinoStore } from './stores/casinoStore';
//import piniaPluginPersistedstate from 'pinia-plugin-persistedstate'

let app = createApp(App)
app.use(router)

const pinia = createPinia();
// pinia.use(piniaPluginPersistedstate)
app.use(pinia)

const authStore = useAuthStore()
authStore.initialize()

const casinoStore = useCasinoStore()
casinoStore.initialize()


app.mount('#app')
