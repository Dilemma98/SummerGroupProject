// filepath: c:\Users\emmaa\Webbutvecklare .NET\Egna projekt\SummerGroupProject\Frontend\src\router\index.ts
import { createRouter, createWebHistory } from 'vue-router';
import type { RouteRecordRaw } from 'vue-router';
import ListPosts from '../components/ListPosts.vue';
import CreatePost from '../pages/CreatePost.vue';

const routes: RouteRecordRaw[] = [
  { path: '/', name: 'Home', component: ListPosts },
  { path: '/create', name: 'create', component: CreatePost },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;