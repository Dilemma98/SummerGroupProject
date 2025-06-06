<script setup lang="ts">
import { ref } from 'vue';

// Type for a Post object
const post = ref({
    title: '',
    content: '',
    author: '',
    image: null as File | null // Initially no image
});

// To handle image uploads, we can use a File object
const image = ref<File | null>(null);
function handleImageUpload(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
        image.value = target.files[0];
    } else {
        image.value = null;
    }
}

// Function to submit the post
async function submitPost() {
    const formData = new FormData();
    formData.append('title', post.value.title);
    formData.append('content', post.value.content);
    formData.append('author', post.value.author);
    if (image.value) {
        formData.append('image', image.value);
    }

    try {
        const response = await fetch('http://localhost:5196/api/posts', {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const result = await response.json();
        console.log('Post submitted successfully:', result);

        // Reset the form after submission
        post.value.title = '';
        post.value.content = '';
        post.value.author = '';
        image.value = null;
    }
    catch (error) {
        console.error('Error submitting post:', error);
    }
}
</script>


<template>
    <div>
        <h2>Uppdatera tramsbyttorna!</h2>
        <form @submit.prevent="submitPost">
            <div>
                <label for="title">Titel:</label>
                <input type="text" id="title" v-model="post.title" required />
            </div>
            <div>
                <label for="content">Innehåll:</label>
                <textarea id="content" v-model="post.content" required></textarea>
            </div>
            <div>
                <label for="author">Författare:</label>
                <input type="text" id="author" v-model="post.author" required />
            </div>
            <div>
                <label for="image">Bild:</label>
                <input type="file" id="image" @change="handleImageUpload" />
            </div>
            <button type="submit">Skicka in</button>
        </form>
    </div>
</template>