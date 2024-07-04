<template>
    <el-scrollbar height="calc(100vh - 100px)">
        <article v-for="article in list">
            <a class="article-image">
                <img class="article-banner" src="../assets/block.jpg" />
            </a>
            <el-col class="article-details">
                <header>
                    <a :href="'#/archive/' + article.id"><h3 class="article-title">{{article.name}}</h3></a>
                    <el-row class="article-category">
                        <el-row class="cats" v-for="cat,index in article.category"><el-link :href="'#/category/' + cat" :underline="false">{{cat}}</el-link> <el-divider direction="vertical" v-if="index < article.category.length-1" /></el-row>
                    </el-row>
                </header>
                <p class="article-substitude">
                    {{article.content.substring(0, 120)}}
                </p>
            </el-col>
        </article>
    </el-scrollbar>
</template>

<script setup lang="js">
    import { ref, onMounted, watch } from 'vue';
    import { useRoute, onBeforeRouteUpdate } from 'vue-router'
    import axios from 'axios'
    const list = ref();
    const route = useRoute();
    const fetchArticles = () => {
        axios.post('api/getArticle')
            .then(response => {
                console.log(response.data);
                list.value = response.data.data;
            })
            .catch(error => console.error('Error fetching articles:', error));
    };
    // 使用 onMounted 钩子在组件挂载时调用 fetchArticles 函数
    onMounted(() => {
        fetchArticles();
    });

    onBeforeRouteUpdate((to, from) => {
        fetchArticles();
    })
</script>

<style scoped>
    .article-details {
        color: ghostwhite;
        max-width:60%;
    }

    article {
        display: flex;
        background-color: #45465e;
        box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.04), 0px 0px 2px rgba(0, 0, 0, 0.06), 0px 0px 1px rgba(0, 0, 0, 0.04);
        max-height: 300px;
        overflow: hidden;
        padding: 20px;
        gap: 20px;
        margin:10px;
    }

    header {
        flex: 1;
        margin: -20px 0;
        display: flex;
        justify-content: flex-start;
        flex-direction: row;
        gap: 15px;
        align-items: center;
    }

        header a {
            text-decoration: none;
            color: ghostwhite
        }

    .article-category {
        justify-content: center;
    }

    .cats {
        display: flex;
        align-items: center;
    }

    .article-image {
        flex: 0 0 auto;
        width: 200px;
        overflow: hidden;
    }

        .article-image img {
            width: 100%;
            height: auto;
            object-fit: cover
        }

    p.article-substitude {
        overflow: hidden;
    }
</style>