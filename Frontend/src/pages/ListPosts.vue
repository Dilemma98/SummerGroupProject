<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import ButtonAddPost from '../components/ButtonAddPost.vue';
import EditPost from '../components/EditPost.vue';
import DeletePost from '../components/DeletePost.vue';
import { useUser } from '../composables/useUser';

// Track the currently open post and image
const openImagePostId = ref<number | null>(null);
const openImageUrl = ref<string | null>(null);

// User information
const { user } = useUser();
const isLoggedIn = computed(() => !!user.value);

// Functions to open/close image overlay
function openImage(post: Post) {
    openImagePostId.value = post.id;
    openImageUrl.value = post.imageUrl;
}
function closeImage() {
    openImagePostId.value = null;
    openImageUrl.value = null;
}

// Remove post from posts array
function removePost(id: number) {
    posts.value = posts.value.filter(p => p.id !== id);
}

// Type for a Post object
interface Post {
    id: number;
    title: string;
    content: string;
    imageUrl: string | null;
    author: string;
    authorImgUrl: string;
    createdAt: string;
}

// Reactive posts array
const posts = ref<Post[]>([]);

// Fetch posts when component is mounted
onMounted(() => {
    fetch('http://localhost:5196/api/posts')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            // Sort posts by creation date descending
            posts.value = data.sort((a: Post, b: Post) =>
                new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
            );
        })
        .catch(error => {
            console.error('There has been a problem with your fetch operation:', error);
        });
});
</script>

<template>
  <ButtonAddPost />
  <div class="posts-container">
    <ul>
      <li v-for="post in posts" :key="post.id">
        <div class="post" :class="{ 'blurred': !isLoggedIn }">
          <h3 class="title">{{ post.title }}</h3>
          <p class="content">{{ post.content }}</p>

          <!-- Post image -->
          <img 
            v-if="post.imageUrl" 
            :src="`http://localhost:5196${post.imageUrl}`" 
            alt="Post image" 
            class="post-image" 
            :class="{ 'blurred': !isLoggedIn }" 
            @click="openImage(post)" 
          />

          <!-- Author info -->
          <p class="author">
            <strong>Författare:</strong> {{ post.author }}
            <img v-if="post.authorImgUrl" class="authorImg" :src="post.authorImgUrl" alt="Författarbild" />
          </p>

          <!-- Date posted -->
          <p class="datePosted" v-if="post.createdAt">
            <strong>Postat:</strong>
            {{ new Date(post.createdAt).toLocaleDateString('sv-SE') }}<br/>
            {{ new Date(post.createdAt).toLocaleTimeString('sv-SE', { hour: '2-digit', minute: '2-digit' }) }}
          </p>

          <!-- Edit and Delete buttons -->
          <div class="edit-or-delete">
            <EditPost :post="post" />
            <DeletePost :post="post" @deleted="removePost" />
          </div>
        </div>
      </li>
    </ul>
  </div>

  <!-- Fullscreen overlay for image -->
  <div v-if="openImageUrl" class="image-overlay" @click="closeImage">
    <img 
      :src="`http://localhost:5196${openImageUrl}`" 
      alt="Post image" 
      class="fullscreen-image" 
    />
  </div>
</template>

<style scoped>
@import url('https://fonts.cdnfonts.com/css/unifrakturmaguntia');

.posts-container {
    width: 90vw;
    margin: auto;
    margin-top: 2em;
}

ul {
    list-style-type: none;
    width: 100%;
    background-color: rgb(232, 231, 229);
    font-family: Georgia, 'Times New Roman', Times, serif;
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 3rem;
    margin: 0 auto;
    padding: 0;
}

li {
    border: 1px solid #ccc;
    padding: 2rem 1.8rem;
    border-radius: 10px;
    background-color: rgb(239, 238, 236);
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    transition: box-shadow 0.3s ease;
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
}

.post-image {
    display: block;
    max-width: 100%;
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
    max-height: 90vh;
    border-radius: 10px;
    box-shadow: 0 8px 32px rgba(0, 0, 0, 0.4);
}

.datePosted {
    font-size: 0.85rem;
    color: #666;
    font-style: italic;
    margin-top: -0.5rem;
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

.blurred {
  filter: blur(5px);
  pointer-events: none;
  user-select: none;
  opacity: 0.8;
  transition: filter 0.3s ease, opacity 0.3s ease;
}

.authorImg {
    width: 2em;
    border-radius: 1em;
    padding-top: 1em;
}

.author {
    font-size: 0.85rem;
    color: #666;
    font-style: italic;
    margin-top: 0.3rem;
    text-align: right;
    letter-spacing: 0.03em;
    display: flex;
    align-items: center; 
    justify-content: flex-end; 
    gap: 0.5em; 
}

.authorImg {
    width: 2em;
    padding-top: 1em; 
    object-fit: cover;
    transform: translateY(-0.40em);
}
</style>
