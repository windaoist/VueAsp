<template>
    <div class="editor">
        <textarea class="input" :value="input" @input="update"></textarea>
        <div class="output" v-html="output"></div>
    </div>
    <el-row :gutter="20"><el-col :span="10"><el-input v-model="title" placeholder="输入文章标题" /></el-col><el-col :span="10"><el-input v-model="category" placeholder="输入文章分类" /></el-col><el-col span="4"><el-button @click="sendToBackend">提交</el-button></el-col></el-row>
</template>
<script setup>
    import { marked } from 'marked'
    import { debounce } from 'lodash-es'
    import { ref, onMounted, watch, computed } from 'vue'
    import { useRoute, onBeforeRouteUpdate } from 'vue-router'
    import axios from 'axios'
    const input = ref('# hello')
    const output = computed(() => marked(input.value))
    const update = debounce((e) => {
        input.value = e.target.value
    }, 100)
    const list = ref();
    const title = ref();
    const category = ref();
    const route = useRoute();
    const fetchArticles = () => {
        axios.post('api/getArticle')
            .then(response => {
                console.log(response.data);
                list.value = response.data.data;
            })
            .catch(error => console.error('Error fetching articles:', error));
    };
    onMounted(() => {
        fetchArticles();
    });

    onBeforeRouteUpdate((to, from) => {
        fetchArticles();
    });
    function getCurrentDate() {
        const currentDate = new Date();
        const year = currentDate.getFullYear();
        let month = currentDate.getMonth() + 1;
        let day = currentDate.getDate();
        if (month < 10) {
            month = '0' + month;
        }
        if (day < 10) {
            day = '0' + day;
        }
        return year + '/' + month + '/' + day;
    }
    function sendToBackend() {
        let requestData = {
            id: list.value.length + 1,
            name: title.value,
            date: getCurrentDate(),
            category: category.value,
            content: input.value
        }
        axios.post('api/write', JSON.stringify(requestData), {
            headers: {
                'accept': '*/*',
                'Content-Type': 'application/json',
            },
        }).then(response => {
            if (response.status == 200) {
                alert('数据已传送至后端');
            } else {
                alert('数据传送失败');
            }
        }).finally(location.reload())
            .catch(error => {
                console.error('将数据传输至后端时出错', error);
                alert('数据传送失败');
            });
    };
</script>
<style scoped>
    body {
        margin: 0;
    }

    .editor {
        height: calc(90vh - 100px);
        display: flex;
    }

    .input,
    .output {
        overflow: auto;
        width: 50%;
        height: 100%;
        background: #284852;
        color: ghostwhite;
        box-sizing: border-box;
        padding: 0 20px;
    }

    .input {
        border: none;
        border-right: 1px solid #ccc;
        resize: none;
        outline: none;
        background-color: #3271ae;
        font-size: 14px;
        color: ghostwhite;
        font-family: 'Monaco', courier, monospace;
        padding: 20px;
    }

    code {
        color: #f66;
    }
</style>