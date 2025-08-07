<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import { useRoute } from 'vue-router';
import ButtonAddPost from '../components/ButtonAddPost.vue';
import EditPost from '../components/EditPost.vue';
import DeletePost from '../components/DeletePost.vue';

const route = useRoute();

// Toggle for image display
const openImagePostId = ref<number | null>(null);

function openImage(postId: number) {
    openImagePostId.value = postId;
}
function closeImage() {
    openImagePostId.value = null;
}

function removePost(id: number) {
    posts.value = posts.value.filter(p => p.id !== id);
}


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

function fetchPosts(){
      // Fetch posts from the backend API
    fetch('http://localhost:5196/api/posts')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
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
}
// Runs when the component is mounted to the DOM
onMounted(() => { 
    fetchPosts(); 
});
watch(
    () => route.fullPath,
    () => {
        fetchPosts();
    }
);
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
                    <img v-if="post.imageUrl" :src="`http://localhost:5196${post.imageUrl}`" alt="Post image"
                        class="post-image" @click="openImage(post.id)" />
                    <div v-if="openImagePostId === post.id" class="image-overlay" @click="closeImage">
                        <img :src="`http://localhost:5196${post.imageUrl}`" alt="Post image" class="fullscreen-image" />
                    </div>
                    <img v-else />
                    <p class="author"><strong>Författare:</strong> {{ post.author }}</p>
                    <p class="datePosted" v-if="post.createdAt">
                        <strong>Postat:</strong>
                        {{
                            new Date(post.createdAt).toLocaleDateString('sv-SE')
                        }}
                        <br/>
                        {{
                            new Date(post.createdAt).toLocaleTimeString('sv-SE', { hour: '2-digit', minute: '2-digit' })
                        }}
                    </p>
                    <div class="edit-or-delete">
                        <EditPost :post="post" />
                        <DeletePost :post="post" @deleted="removePost" />
                    </div>
                </div>
            </li>
        </ul>
    </div>
</template>

<style scoped>
@import url('https://fonts.cdnfonts.com/css/unifrakturmaguntia');

.posts-container {
    width: 60vw;
    margin: auto;
    margin-top: 2em;
}

ul {
    list-style-type: none;
    width: 70%;
    background-color: rgb(232, 231, 229);
    font-family: Georgia, 'Times New Roman', Times, serif;
    margin: 0 auto;
   
}

li {
    border: 1px solid #ccc;
    padding: 2rem 1.8rem;
    border-radius: 10px;
    background-color: rgb(239, 238, 236);
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    transition: box-shadow 0.3s ease;
    margin-top: 2em;
}

li:hover {
    box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);
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
    margin: auto;
}

.post-image {
    display: block;
    max-width: 80%;
    height: auto;
    margin: 1.5rem auto;
    border-radius: 6px;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
    cursor: pointer;
    transition: box-shadow 0.3s;
}

.image-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.256);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
}

.fullscreen-image {
    max-width: 90vw;
    height: 95vh;
    border-radius: 10px;
    box-shadow: 0 8px 32px rgba(0, 0, 0, 0.4);
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

.edit-or-delete {
    width: 7em;
    align-items: flex-start;
    display: flex;
    gap: 2rem;
    margin-top: 3rem;
}
</style>
