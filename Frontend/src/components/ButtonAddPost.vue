<script setup lang="ts">

</script>

<template>
  <div class="button-container">
    <router-link to="/create" class="add-post-button">
      Skriv något nytt!
    </router-link>
  </div>
</template>

<style scoped>
.button-container {
  text-align: end;
  margin: 2rem 4rem;
}

.add-post-button {
  display: inline-block;
  padding: 0.75rem 1.5rem;
  background-color: #c9c9c9;
  color: black;
  font-size: 1.2rem;
  font-weight: bold;
  text-decoration: none;
  border-radius: 8px;
  transition: background-color 0.3s ease, transform 0.2s ease;
  font-family: Georgia, 'Times New Roman', Times, serif;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.add-post-button:hover {
  background-color: #959595;
  color: white;
  transform: translateY(-2px);
  cursor: pointer;
}
</style>