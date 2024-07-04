<script setup>
    import { ref, computed, onMounted } from 'vue'
    import axios from 'axios'
    import { useRoute, useRouter } from 'vue-router'
    const router = useRouter()
    const route = useRoute()
    var loginDisplay = ref(false)
    const loggedIn = ref(false)
    var formData = {
        account: "",
        password: ""
    }
    const key = ref(0);
    const search = computed({
        get() {
            return route.query.search ?? ''
        },
        set(search) {
            router.push({ query: { search } })
            key.value++;
        }
    })
    async function login() {
        axios.post('api/login', formData, {
            headers: {
                'accept': '*/*',
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                console.log('Response:', response.data);
                // 处理成功响应
                if (response.data.message === 'Login successful') {
                    // 登录成功逻辑，例如保存登录状态到Vuex或localStorage
                    console.log('Login successful');
                    localStorage.setItem('account', response.data.session)
                    loggedIn.value = true;
                    loginDisplay.value = false;
                } else {
                    // 其他情况，例如处理错误消息
                    console.log('Login failed');
                    alert("登录失败")
                }
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }
    async function logout() {
        try {
            const response = await axios.post('api/logout');
            console.log('Logout successful:', response.data);
            // 在成功注销后，清除前端保存的Session信息
            localStorage.removeItem('account');
            loggedIn.value = false;
        } catch (error) {
            console.error('Logout failed:', error);
        }
    }
    onMounted(() => {
        const account = localStorage.getItem('account');
        if (account) {
            // 恢复登录状态
            loggedIn.value = true;
            loginDisplay.value = false;
        }
    });
</script>

<template>
    <div class="common-layout">
        <div id="loginOverlay" v-if="loginDisplay">
            <div class="popup">
                <p class="popup-title">登录</p>
                <el-form class="login-area">
                    <label>账号<input id="account" type="text" v-model="formData.account" /></label>
                    <label>密码<input id="password" type="password" v-model="formData.password"></label>
                </el-form>
                <div class="popup-btn">
                    <el-button class="cancelBtn" @click="loginDisplay=!loginDisplay">取消</el-button>
                    <el-button class="confirmBtn" @click="login">登录</el-button>
                </div>
            </div>
        </div>
        <el-container>
            <el-aside width="200px">
                <el-button @click="loginDisplay=!loginDisplay" v-if="!loggedIn">登录</el-button>
                <el-button @click="logout" v-if="loggedIn">注销</el-button>
                <ul class="sidebar-menu">
                    <li>
                        <router-link to="/">
                            <el-icon color="ghostwhite"><House /></el-icon>
                            <span class="menu">首页</span>
                        </router-link>
                    </li>
                    <li>
                        <router-link to="/edit" v-if="loggedIn">
                            <el-icon color="ghostwhite"><EditPen /></el-icon>
                            <span class="menu">撰写</span>
                        </router-link>
                    </li>
                    <li>
                        <router-link to="/admin" v-if="loggedIn">
                            <el-icon color="ghostwhite"><Link /></el-icon>
                            <span class="menu">后台</span>
                        </router-link>
                    </li>
                </ul>
            </el-aside>
            <el-container>
                <el-header>
                    <h2 class="header-title">欢迎！</h2>
                </el-header>
                <el-main>
                    <RouterView :key="key" />
                </el-main>
            </el-container>
        </el-container>
    </div>
</template>

<style>
    body {
        background-color: #00283a;
    }

    .el-aside {
        margin-left: 100px;
        padding-top: 35px;
        padding-bottom: 35px;
        height: calc(110vh - 100px);
        display: flex;
        flex-direction: column;
        background-color: #45465e;
        box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.04), 0px 0px 2px rgba(0, 0, 0, 0.06), 0px 0px 1px rgba(0, 0, 0, 0.04);
    }

        .el-aside img {
            border: 5px solid black;
            width: 120px;
            margin: 0 auto;
            border-radius: 120px;
        }
        .el-aside .el-button {
            width: 100px;
            margin: 10px auto;
        }
    span.menu {
        color: ghostwhite
    }

    .sidebar-menu {
        list-style: none;
        padding: 0;
        margin: 0;
        width: 200px; /* 设置侧边栏的宽度 */
    }

        .sidebar-menu li a {
            display: flex; /* 使用flex布局 */
            align-items: center; /* 垂直居中对齐 */
            padding: 15px 30px;
            text-decoration: none; /* 去掉下划线 */
            color: #333; /* 字体颜色 */
            transition: background-color 0.3s; /* 背景颜色过渡效果 */
            gap: 25%;
        }

            .sidebar-menu li a:hover {
                background-color: #9aa7b1; /* 悬停时的背景颜色 */
            }

        .sidebar-menu li a {
            margin: 10px auto;
        }

    .header-title {
        position: relative;
        margin: auto 0;
        text-align: center;
        margin: 20px;
        background-color: #45465e;
        color: ghostwhite;
        padding: 10px 0px;
        box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.04), 0px 0px 2px rgba(0, 0, 0, 0.06), 0px 0px 1px rgba(0, 0, 0, 0.04);
    }

    .el-main {
        display: flex;
        justify-content: space-start;
        flex-direction: column;
        margin-top: 20px;
        padding: 0 40px;
        gap: 20px;
    }

    div#loginOverlay {
        position: fixed;
        left: 0px;
        top: 0px;
        width: 100%;
        height: 100%;
        font-size: 16px;
        background-color: rgba(0, 0, 0, 0.5);
        z-index: 9999;
    }

    .login-area {
        display: flex;
        flex-direction: column
    }

        .login-area label,
        .login-area label input {
            display: inline-block;
            margin: 5px 20px;
        }
    /* 弹出框主体 */
    .popup {
        background-color: #ffffff;
        max-width: 400px;
        min-width: 200px;
        height: 240px;
        border-radius: 5px;
        margin: 10% auto;
        text-align: center;
    }
    /* 弹出框的标题 */
    .popup-title {
        height: 60px;
        line-height: 60px;
        border-bottom: solid 1px #cccccc;
    }
    /* 弹出框的内容 */
    .popup_content {
        height: 50px;
        line-height: 50px;
        padding: 15px 20px;
    }
    /* 弹出框的按钮栏 */
    .popup-btn {
        margin: 0px 20px;
    }
</style>