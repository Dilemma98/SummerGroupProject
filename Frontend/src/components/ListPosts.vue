<script setup lang="ts">
import { ref, onMounted } from 'vue';
import ButtonAddPost from './ButtonAddPost.vue';
// Type for a Post object
interface Post {
    id: number;
    title: string;
    content: string;
    imageUrl: string | null; // URL to the image, or null if no image
    author: string;
    createdAt: string; // ISO date string
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
            // Sort posts by createdAt in descending order
            posts.value = data.sort((a: Post, b: Post) => 
            new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        })
        .catch(error => {
            // Log any error that occurs during the fetch
            console.error('There has been a problem with your fetch operation:', error);
        });
});
</script>

<template>
    <ButtonAddPost />
    <div class="posts-container">
        <ul>
            <!-- Loop through posts and display each one -->
            <li v-for="post in posts" :key="post.id">
                <div class="post">
                    <h3 class="title">{{ post.title }}</h3>
                    <p class="content">{{ post.content }}</p>
                    <img v-if="post.imageUrl" :src="`http://localhost:5196${post.imageUrl}`" alt="Post image" />
                    <img v-else />
                    <p class="author"><strong>Författare:</strong> {{ post.author }}</p>
                    <p class="datePosted"><strong>Datum:</strong> {{ new
                        Date(post.createdAt).toLocaleDateString('sv-SE') }}
                    </p>
                </div>
            </li>
        </ul>
    </div>
</template>

<style scoped>
@import url('https://fonts.cdnfonts.com/css/unifrakturmaguntia');

.posts-container {
    width: 80vw;
    margin: auto;
}
ul {
    list-style-type: none;
    
    margin: 0 auto;
    width: 75%;
    background-color: #fff;
    font-family: Georgia, 'Times New Roman', Times, serif;
    
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
    gap: 3rem;
}

li {
    border: 1px solid #ccc;
    padding: 2rem 1.8rem;
    border-radius: 10px;
    background-color: #fcfcfc;

    box-shadow: 0 4px 8px rgba(0,0,0,0.1);
    transition: box-shadow 0.3s ease;
}

li:hover {
    box-shadow: 0 8px 20px rgba(0,0,0,0.15);
}

.title {
    font-family: 'Georgia', serif;
    font-size: 2.4rem;
    font-weight: 600;
    margin-bottom: 0.4rem;
    color: #222;
    text-align: left;
    line-height: 1.2;
}

.post {
    font-family: Georgia, 'Times New Roman', Times, serif;
    font-size: 1.15rem;
    line-height: 1.8;
    color: #333;
    max-width: 90%;
}

img {
    display: block;
    max-width: 100%;
    height: auto;
    margin: 1.5rem auto;
    border-radius: 6px;
    box-shadow: 0 2px 6px rgba(0,0,0,0.1);
    transition: transform 0.3s ease;
}
img:hover {
    transform: scale(2.7); 
    z-index: 10;
}

.author,
.datePosted {
    font-size: 0.85rem;
    color: #666;
    font-style: italic;
    margin-top: 0.3rem;
    text-align: right;
    letter-spacing: 0.03em;
}

</style>
