import { createRouter, createWebHashHistory } from 'vue-router'
import BlogPost from './components/BlogPost.vue'
import Category from './components/Category.vue'
import Article from './components/Article.vue'
import About from './components/About.vue'
import Edit from './components/Edit.vue'
import NotFound from './components/NotFound.vue'
import Admin from './components/Admin.vue'
import Change from './components/Change.vue'

const routes = [
    { path: '/',name:'home', component: BlogPost },
    { path: '/about', name: 'about', component: About },
    { path: '/category/:cat?', name: 'category', component: Category,props:true },
    { path: '/archive/:id', name: 'archive', component: Article },
    {path:'/edit',name:'edit',component:Edit},
    { path: '/:pathMatch(.*)*', name: 'notfound', component: NotFound }, 
    { path: '/admin', name: 'admin', component: Admin },
    {path:'/change/:id',name:'change',component:Change}
]

const router = createRouter({
    history: createWebHashHistory(),
    routes
})

export default router
