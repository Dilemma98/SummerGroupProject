<script setup lang="ts">
import CreatePost from './components/CreatePost.vue';
import Header from './components/Header.vue';
import ListPosts from './components/ListPosts.vue';
</script>

<template>
  <div>
    <Header />
    <CreatePost />
    <ListPosts />
  </div>
</template>

<style scoped>
.logo {
  height: 6em;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;
}
.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}
.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}
</style>
