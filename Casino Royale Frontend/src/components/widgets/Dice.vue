<template>
  <section class="dice-section">
    <div class="scene">
      <div class="dice" ref="diceRef">
        <div class="face one">1</div>
        <div class="face two">2</div>
        <div class="face three">3</div>
        <div class="face four">4</div>
        <div class="face five">5</div>
        <div class="face six">6</div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, watch } from 'vue';

const rotations = {
  1: { x: 0, y: 0 },
  2: { x: 0, y: 180 },
  3: { x: 0, y: -90 },
  4: { x: 0, y: 90 },
  5: { x: -90, y: 0 },
  6: { x: 90, y: 0 },
};

let props = defineProps({
    spinDice: Number
});

watch(() => props.spinDice, (newVal) => {
  if (typeof newVal === 'number' && newVal >= 1 && newVal <= 6) {
    spinDice(newVal);
  } else if (newVal) {
    spinDice();
  }
});

const diceRef = ref(null);
let spinCount = 0;
const face = ref(1);

function spinDice(toFace) {
  if (typeof toFace === 'number' && toFace >= 1 && toFace <= 6) {
    face.value = toFace;
  } else {
    face.value = Math.floor(Math.random() * 6) + 1;
  }
  spinCount++;
  const spinX = 360 * 3;
  const spinY = 360 * 3;
  const rotation = rotations[face.value];
  const totalX = spinX * spinCount + rotation.x;
  const totalY = spinY * spinCount + rotation.y;
  if (diceRef.value) {
    diceRef.value.style.transform = `rotateX(${totalX}deg) rotateY(${totalY}deg)`;
  }
}

</script>

<style scoped>
.dice-section {
  width: 150px;
  height: 150px;
  display: inline-block;
  vertical-align: middle;
}
.scene {
  width: 100px;
  height: 100px;
  perspective: 600px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.dice {
  width: 100px;
  height: 100px;
  position: relative;
  transform-style: preserve-3d;
  transition: transform 1s ease;
}
.face {
  position: absolute;
  width: 100px;
  height: 100px;
  background: white;
  color: black;
  font-size: 48px;
  display: flex;
  justify-content: center;
  align-items: center;
  border: 2px solid #000;
}
.one {
  transform: rotateY(0deg) translateZ(50px);
}
.two {
  transform: rotateY(180deg) translateZ(50px);
}
.three {
  transform: rotateY(90deg) translateZ(50px);
}
.four {
  transform: rotateY(-90deg) translateZ(50px);
}
.five {
  transform: rotateX(90deg) translateZ(50px);
}
.six {
  transform: rotateX(-90deg) translateZ(50px);
}
</style>
