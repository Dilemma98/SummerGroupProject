import { ref } from 'vue';
const savedUser = localStorage.getItem('user');
export const user = ref<any>(savedUser ? JSON.parse(savedUser) : null);

export function useUser() {
  return { user };
}