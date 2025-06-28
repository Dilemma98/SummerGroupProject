<script setup lang="ts">
import Header from './components/Header.vue';
</script>

<template>
  <div>
    <Header />
    <router-link to="/create"></router-link>
    <router-view />
  </div>
</template>

<style>
body{
  background-color: rgb(232, 231, 229);
}
</style>

