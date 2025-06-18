<script setup lang="ts">
import Header from './components/Header.vue';
</script>

<template>
  <div>
    <Header />
    <router-link to="/create"></router-link>
    <router-view /> <!-- Här visas rätt sida beroende på route -->
  </div>
</template>
<style>

</style>
