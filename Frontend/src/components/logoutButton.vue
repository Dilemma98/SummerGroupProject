<script setup lang="ts">
import { useUser } from '../composables/useUser';
import { useRouter } from 'vue-router';
const { user } = useUser();
const router = useRouter();

function logout() {
    localStorage.removeItem('user');
    user.value = null;
    router.push('/');
}
</script>

<template>
    <button class="logout-button" @click="logout">
        Logga ut
    </button>
</template>

<style scoped>
.logout-button {
    background-color: #c9c9c9;
    color: #222;
    font-size: 1.1rem;
    font-family: Georgia, 'Times New Roman', Times, serif;
    font-weight: bold;
    border: none;
    border-radius: 8px;
    padding: 0.6rem 1.4rem;
    margin-left: 1rem;
    cursor: pointer;
    transition: background-color 0.2s, color 0.2s, transform 0.2s;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.08);
}

.logout-button:hover {
    background-color: #959595;
    color: #fff;
    transform: translateY(-2px);
}
</style>