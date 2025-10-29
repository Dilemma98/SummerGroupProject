<script setup lang="ts">
import GoogleLoginButton from './googleLoginButton.vue';
import LogoutButton from './logoutButton.vue';
import { useUser } from '../composables/useUser';
const { user } = useUser();
</script>

<template>
  <header class="newspaper-header">
    <router-link to="/" class="newspaper-title">Tramsrapporten</router-link>
    <div v-if="!user || !user.email" class="button-container">
      <GoogleLoginButton />
    </div>
    <div v-if="user && user.email" class="welcomeUserText">
      <h3>Välkommen {{user.name}}! <img class="userImg" :src="user.picture" alt=""></h3>
    </div>
    <LogoutButton v-if="user && user.email" />
  </header>
</template>

<style scoped>
@import url('https://fonts.cdnfonts.com/css/unifrakturmaguntia');

.newspaper-header {
  border-bottom: 2px solid #333;
  margin-bottom: 2rem;
  margin-top: -1rem;
  width: 90%;
  margin: auto;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 2rem; 
}

.newspaper-title {
  font-family: 'UnifrakturMaguntia', cursive;
  font-size: 5rem;
  color: #333;
  margin: 0;
}

.button-container {
  margin: 0;
}

.welcomeUserText {
  font-family: Georgia, 'Times New Roman', Times, serif;
  font-size: 1.2rem;
  color: #333;
  margin-left: 7em;
  display: flex;
  align-items: center;
  gap: 1rem;
}

.userImg {
  border-radius: 50%;
  height: 2.5em;
  margin-left: 0.5em;
  transform: translateY(0.5em);
}
</style>
