import { createRouter, createWebHistory } from 'vue-router';
import Home from '../pages/Home.vue';
import About from '../pages/About.vue';
import Login from '../pages/Login.vue';
import Signup from '../pages/Signup.vue';
import Room from '../pages/Room.vue';
import { requireAuth, notRequireAuth } from './gaurd.js';

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: '/', component: Home, name: 'Home' },
        { path: '/about', component: About, name: 'About' },
        { path: '/login', component: Login, name: 'Login', beforeEnter: [notRequireAuth] },
        { path: '/signup', component: Signup, name: 'Signup', beforeEnter: [notRequireAuth] },
        { path: '/room/:id', component: Room, name: 'Room', beforeEnter: [requireAuth] }
    ]
});

export default router;