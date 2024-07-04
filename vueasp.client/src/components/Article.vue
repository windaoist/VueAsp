<template>
    <div v-if="!isLoading">
        <header>
            <h2>{{article.name}}</h2>
            <el-row class="cats" v-for="cat,index in article.category"><el-link type="info" :href="'#/category/' + cat" :underline="false">{{cat}}</el-link> <el-divider direction="vertical" v-if="index < article.category.length-1" /></el-row>
        </header>
        <div class="content" v-html="content"></div>
    </div>
</template>
<script setup>
    import { ref, computed, onMounted } from 'vue';
    import { useRoute, onBeforeRouteUpdate } from 'vue-router';
    import { marked } from 'marked'
    import axios from 'axios'
    const route = useRoute();
    const article = ref();
    var content = ref();
    const isLoading=ref(true);
    const fetchArticle = () => {
        axios.get('api/article', {
            headers: {
                'accept': '*/*',
                'id': Number(route.params.id)
            }
        })
            .then(response => {
                console.log(response.data);
                article.value = response.data.data.result;
            })
            .catch(error => console.error('获取文章失败:', error))
            .finally(isLoading.value = false)

        content = computed(() => marked(article.value.content))
    };
    onMounted(() => {
        fetchArticle();
    });

    onBeforeRouteUpdate((to, from) => {
        fetchArticle();
    })
    //   const article = computed(() => {
    //       const articleId = Number(route.params.id);
    //       return props.list.find(item => item.id === articleId);
    //   });

</script>
<style scoped>
    header {
        display: flex;
        flex-direction: row;
        justify-items: center
    }

    h2 {
        margin-right: 20px;
        color: ghostwhite
    }

    .cats {
        display: flex;
        align-items: center;
    }

    div.content {
        color: ghostwhite
    }
</style>