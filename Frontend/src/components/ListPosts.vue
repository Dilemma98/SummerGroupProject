<script setup lang="ts">
import { ref, onMounted } from 'vue';

// Type for a Post object
interface Post {
    id: number;
    title: string;
    content: string;
    author: string;
    date: string; // ISO date string
}

// Reactive variable to store posts
const posts = ref<Post[]>([]);

// Runs when the component is mounted to the DOM
onMounted(() => {
    // Fetch posts from the backend API
    fetch('http://localhost:5196/api/posts')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
             console.log(response);
            return response.json();
        })
        .then(data => {
            // Update the reactive posts variable
            posts.value = data;
        })
        .catch(error => {
            // Log any error that occurs during the fetch
            console.error('There has been a problem with your fetch operation:', error);
        });
});
</script>

<template>
    <div>
        <ul>
            <!-- Loop through posts and display each one -->
            <li v-for="post in posts" :key="post.id">
                <h3 class="title">{{ post.title }}</h3>
                <p class="content">{{ post.content }}</p>
                <p class="author"><strong>Author:</strong> {{ post.author }}</p>
                <p class="datePosted"><strong>Date:</strong> {{ new Date(post.date).toLocaleDateString() }}</p>
            </li>
        </ul>
    </div>
</template>

<style scoped>
ul {
    list-style-type: none;
    padding: 0;
}

.title {
    color: #333;
    font-size: 1.5em;
}
.content {
    color: #666;
    font-size: 1.2em;
}
.author {
    color: #999;
    font-style: italic;
}
.datePosted {
    color: #ccc;
    font-size: 0.9em;
}
</style>
