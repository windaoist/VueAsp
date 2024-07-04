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
                    {{article.content.substring(0,120)}}
                </p>
            </el-col>
            <el-row class="article-buttons">
                <el-button type="primary" size="small" @click="editArticle(article.id)">编辑</el-button>
                <el-button type="danger" size="small" @click="deleteArticle(article.id)">删除</el-button>
            </el-row>
        </article>
    </el-scrollbar>
</template>

<script setup lang="js">
    import { ref, onMounted, watch } from 'vue';
    import { useRoute, useRouter, onBeforeRouteUpdate } from 'vue-router'
    import axios from 'axios'
    const list = ref();
    const route = useRoute();
    const router = useRouter();
    const fetchArticles = () => {
        axios.post('api/getArticle')
            .then(response => {
                console.log(response.data);
                list.value = response.data.data;
            })
            .catch(error => console.error('查找文章失败', error));
    };
    const editArticle = (id) => {
        router.push({ name: 'change', params: { id } });
    };
    const deleteArticle = (id) => {
        axios.get('api/delete', {
            headers: {
                'accept': '*/*',
                'id': id
            }
        })
            .then(response => {
            })
            .catch(error => console.error('获取文章失败:', error));
        location.reload()
    };
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
        max-width: 60%;
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