import { defineStore } from "pinia";
import { reactive } from "vue";

export const useCasinoStore = defineStore("casinoStore", () => {
  const roomDetails = reactive({
    roomId: "",
    roomName: "",
    maxPeople: 0,
    initialAmount: 0,
    percentSeven: 0,
    percentOther: 0
  });

  function setRoomDetails(details) {
    roomDetails.roomId = details.roomId;
    roomDetails.roomName = details.roomName;
    roomDetails.maxPeople = details.maxPeople;
  }

  function initialize() {
    // Initialize room details if needed
    setRoomDetails({
      roomId: "",
      roomName: "",
      maxPeople: 0,
    });
  }

  return {
    roomDetails,
    setRoomDetails,
    initialize,
  };
});
