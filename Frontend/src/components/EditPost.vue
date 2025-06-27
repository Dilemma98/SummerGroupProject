<script setup lang="ts">
import { ref } from 'vue';
import { useUser } from '../composables/useUser';
const { user } = useUser();

// Type for a Post object
const { post } = defineProps<{
  post: {
    author: string;
    title: string;
    content: string;
    id: number;
    imageUrl?: string | null;
    createdAt?: string;
  };
}>();

const isEditing = ref(false);
const editTitle = ref(post.title);
const editContent = ref(post.content);

function handleEdit() {
  // Logic to handle post editing
  console.log('Edit post:', post.id);
  isEditing.value = true;

}

function updatePost() {
  // Logic to update the post
  console.log('Updating post:', post.id, editTitle.value, editContent.value);

  // Here you would typically send the updated data to your backend API
  fetch(`http://localhost:5196/api/posts/${post.id}`, {
    method: 'PATCH',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      id: post.id,
      title: editTitle.value,
      content: editContent.value,
      author: post.author,
      imageUrl: post.imageUrl,
      createdAt: post.createdAt
    })
  })
    .then(res => {
      if (!res.ok) throw new Error('Network response was not ok');
      // Gör inget mer här om du inte behöver svarsdatan
    })

    .then(() => {
      isEditing.value = false; // Exit editing mode
      post.title = editTitle.value; // Update the post title
      post.content = editContent.value; // Update the post content
    })
    .catch(error => {
      console.error('Error updating post:', error);
    });
}

</script>

<template>
  <!-- Checks if user is signed in and is the same as author of post -->
  <div v-if="user && user.email && user.name === post.author">
    <i class="fa-solid fa-pencil" @click="handleEdit"></i>
  </div>
  <div v-if="isEditing">
    <input v-model="editTitle" placeholder="Edit title" />
    <textarea v-model="editContent" placeholder="Edit content"></textarea>
    <i class="fa-solid fa-floppy-disk" @click="updatePost"></i>
  </div>
</template>

<style scoped>
.fa-pencil {
  cursor: pointer;
  color: #333;
  font-size: 1.5rem;
}

.fa-floppy-disk {
  cursor: pointer;
  color: #333;
  font-size: 1.5rem;
  margin-left: 0.5rem;
}
</style>