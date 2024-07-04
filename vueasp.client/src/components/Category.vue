<template>
    <header class="category-header">
        <el-row><h2>分类</h2><span>{{cat}}</span></el-row>
    </header>
    <article v-for="article in list">
        <a class="article-image">
            <img class="article-banner" src="../assets/block.jpg" />
        </a>
        <el-col class="article-details">
            <header class="article-header">
                <a :href="'#/archive/' + article.id"><h3 class="article-title">{{article.name}}</h3></a>
                <el-row class="article-category">
                    <el-row class="cats" v-for="cat,index in article.category"><el-link :href="'#/category/' + cat" :underline="false">{{cat}}</el-link> <el-divider direction="vertical" v-if="index < article.category.length-1" /></el-row>
                </el-row>
            </header>
            <h4 class="article-substitude">
                {{article.content}}
            </h4>
        </el-col>
    </article>
</template>
<script setup>
    import { ref, computed, onMounted } from 'vue';
    import { useRoute, onBeforeRouteUpdate } from 'vue-router';
    import axios from 'axios'
    const route = useRoute();
    const list = ref();
    const cat = route.params.cat;
    const fetchArticles = () => {
        axios.post('api/getArticle')
            .then(response => {
                console.log(response.data);
                list.value = response.data.data.filter(article => shouldRender(article));
            })
            .catch(error => console.error('Error fetching articles:', error));
    };
    onMounted(() => {
        fetchArticles();
    });

    onBeforeRouteUpdate((to, from) => {
        fetchArticles();
    })
    const shouldRender = (article) => {
        // Example condition: render articles from the 'tech' category
        return article.category.includes(cat);
    };
</script>
<style scoped>
    header.category-header {
        display: flex;
        flex-direction: row;
    }

        header.category-header .el-row {
            align-items: center;
        }
        header.category-header .el-row span{
            color:ghostwhite
        }

    h2 {
        margin-right: 20px;
        color: ghostwhite
    }

    .cats {
        display: flex;
        align-items: center;
    }

    article {
        display: flex;
        background-color: #45465e;
        box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.04), 0px 0px 2px rgba(0, 0, 0, 0.06), 0px 0px 1px rgba(0, 0, 0, 0.04);
        max-height: 300px;
        overflow: hidden;
        padding: 20px;
        gap: 20px;
    }

    header.article-header {
        flex: 1;
        margin: -20px 0;
        display: flex;
        justify-content: flex-start;
        flex-direction: row;
        gap: 15px;
        align-items: center;
    }

        header.article-header a {
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
    .article-details {
        color: ghostwhite
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
</style>