<template>
  <Portal v-if="currentModal">
    <component :is="modal" />
  </Portal>
  
  <Layout>
    <router-view />
  </Layout>
</template>

<script setup>
import { ref, provide, computed } from "vue";

import Portal from "./components/layouts/Portal.vue";
import Layout from "./components/layouts/Layout.vue";
import CreateRoom from "./components/pages/sub/CreateRoom.vue";
import JoinRoom from "./components/pages/sub/JoinRoom.vue";
import RoomDetail from "./components/pages/sub/RoomDetail.vue";
import connection from "@/clients/signalr";

connection.start();
const currentModal = ref(null);

const modal = computed(() => {
  switch (currentModal.value) {
    case "CreateRoom":
      return CreateRoom;
    case "JoinRoom":
      return JoinRoom;
    case "RoomDetail":
      return RoomDetail;
    default:
      return null;
  }
});

provide("currentModal", currentModal);
</script>
