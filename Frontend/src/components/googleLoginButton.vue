<script setup lang="ts">
import { onMounted } from 'vue';
import { useUser } from '../composables/useUser';
const { user } = useUser();

const clientId = import.meta.env.VITE_GOOGLE_CLIENT_ID;

function handleCredentialResponse(response: any) {
    const idToken = response.credential;
    fetch('http://localhost:5196/api/Google/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(idToken)
    })
        .then(res => res.json())
        .then(data => {
            user.value = data;
            localStorage.setItem('user', JSON.stringify(data));
        })
        .catch(err => {
            console.error('Fel vid inloggning:', err);
        });
}

onMounted(() => {
    if (!clientId) {
        console.error('Google Client ID saknas!');
        return;
    }
    // @ts-ignore
    if (window.google && window.google.accounts && window.google.accounts.id) {
        // @ts-ignore
        window.google.accounts.id.initialize({
            client_id: clientId,
            callback: handleCredentialResponse
        });
        // @ts-ignore
        window.google.accounts.id.renderButton(
            document.getElementById('googleSignInDiv'),
            { theme: 'outline', size: 'large' }
        );
    } else {
        console.error('Google Identity Services-scriptet är inte laddat!');
    }
});
</script>

<template>
    <div v-if="!user || !user.email" id="googleSignInDiv"></div>
</template>

<style scoped>
#googleSignInDiv {
    margin: 2rem auto;
    text-align: center;
    width: 13em;
}
</style>