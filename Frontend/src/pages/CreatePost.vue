<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useUser } from '../composables/useUser';
const { user } = useUser();
// Importing the router to navigate after submission
const router = useRouter();

// Type for a Post object
const post = ref({
  title: '',
  content: '',
  author: user.value.name,
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



    // Reset the form after submission
    post.value.title = '';
    post.value.content = '';
    post.value.author = '';
    image.value = null;

    // Redirect to the home page 
    router.push('/');
  }
  catch (error) {
    console.error('Error submitting post:', error);
  }
}
</script>

<template>
  <div class="form-container">
    <h2 class="form-title">Uppdatera dina fellow tramsbyttor!</h2>
    <form @submit.prevent="submitPost" class="post-form">
      <div class="form-group">
        <input type="text" id="title" v-model="post.title" required class="form-input" placeholder="Titel" />
      </div>
      <div class="form-group">
        <textarea id="content" v-model="post.content" required class="form-textarea" placeholder="Innehåll"></textarea>
      </div>
      <div class="form-group">
        <span class="form-value">{{ post.author }}</span>
      </div>
      <div class="form-group">
        <!-- <label for="image" class="form-label">Bild:</label> -->
        <input type="file" id="image" @change="handleImageUpload" class="form-input-file" />
      </div>
      <button type="submit" class="form-button">Skicka in</button>
    </form>
  </div>
</template>

<style scoped>
@import url('https://fonts.cdnfonts.com/css/unifrakturmaguntia');

.form-container {
  width: 90vw;
  margin: 2rem auto;
  font-family: Georgia, 'Times New Roman', Times, serif;
  background-color: #fcfcfc;
  padding: 2rem 3rem;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  max-width: 600px;
  text-align: center;
}

h2 {
  font-family: 'Georgia', serif;
  font-size: 2.4rem;
  font-weight: 600;
  margin-bottom: 1.5rem;
  color: #222;
  text-align: center;
}

.post-form {
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}

label {
  font-weight: 600;
  font-size: 1rem;
  color: #333;
}

input[type="text"],
textarea,
input[type="file"] {
  font-family: Georgia, serif;
  font-size: 1rem;
  padding: 0.5rem 0.75rem;
  border-radius: 6px;
  border: 1px solid #ccc;
  transition: border-color 0.3s ease;
}

input[type="text"]:focus,
textarea:focus {
  border-color: #888;
  outline: none;
}

textarea {
  min-height: 100px;
  resize: vertical;
}

button {
  width: fit-content;
  align-self: center;
  background-color: #555;
  color: white;
  font-size: 1.1rem;
  padding: 0.6rem 2rem;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
  transition: background-color 0.3s ease, box-shadow 0.3s ease;
}

button:hover {
  background-color: #333;
  box-shadow: 0 6px 14px rgba(0, 0, 0, 0.25);
}
</style>
