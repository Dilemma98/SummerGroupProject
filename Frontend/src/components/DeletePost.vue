<script setup lang="ts">
import { useUser } from '../composables/useUser';
const { user } = useUser();

const emit = defineEmits(['deleted']);

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

function handleDelete() {
  // Logic to handle post deletion
  console.log('Delete post:', post.id);
  fetch(`http://localhost:5196/api/posts/${post.id}`, {
    method: 'DELETE',
  })
    .then(response => {
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      // Optionally, you can emit an event or update the state to remove the post from the UI
      console.log('Post deleted successfully');
      emit('deleted', post.id);
    })
    .catch(error => {
      console.error('Error deleting post:', error);
    });
}

</script>

<template>
    <!-- Checks if user is signed in and is the same as author of post -->
 <div v-if="user && user.email && user.name === post.author">
    <i class="fa-solid fa-trash" @click="handleDelete"></i>
  </div>
</template>

<style scoped>
.fa-trash {
  cursor: pointer;
  color: #333;
  font-size: 1.5rem;
}
</style>