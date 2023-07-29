<template>
    <v-navigation-drawer
        app
        permanent
    >
        <v-list class="text-center">
            <v-list-item class="px-8">
                <v-list-item-avatar>
                </v-list-item-avatar>
                <v-avatar :size="80" class="align-center avatar" style="child-align: right !important;">
                    <v-img :src="require('@/assets/logo-small2.svg')" ></v-img>
                </v-avatar>
            </v-list-item>
            <v-list-item>
                <v-list-item-content>
                    <v-list-item-title class="text-h6">{{ getUsername }}</v-list-item-title>
                    <v-list-item-subtitle>{{ getNameParceiro }}</v-list-item-subtitle>
                    <v-list-item-title class="text-sm-h6 mt-4 linkItem" @click="linkTo('/')">
                        <v-icon color="primary" >mdi-exit-to-app</v-icon>
                        Sair
                    </v-list-item-title>
                </v-list-item-content>
            </v-list-item>
        </v-list>

        <v-divider></v-divider>

        <v-list dense nav>
            <v-list-item
                v-for="item in items"
                :key="item.title"
                link
            >
                <v-list-item-icon>
                    <v-icon style="color: #FF6A13; " @click="linkTo(item.link)">{{ item.icon }}</v-icon>
                </v-list-item-icon>
                <v-list-item-content class="linkItem itensMenuLateral" @click="linkTo(item.link)">
                    <v-list-item-title style="font-size: 15px;padding-left: 1px;" >{{ item.title }}</v-list-item-title>
                </v-list-item-content>
            </v-list-item>
        </v-list>
    </v-navigation-drawer>
</template>

<script>
export default {
    data () {
        return {
            items: [],
            Nome: "",
            SobreNome: "",
            Parceiro: "",
            hasPermissionAllMenu: false,
        }
    },
    props: {
    },
    computed: {
        getUsername() {
            this.getNomeSobrenome();
            return this.Nome + " " + this.SobreNome;
        },
        getNameParceiro(){
            this.getParceiro();
            return this.Parceiro;
        },
    },

    mounted() {
        this.hasPermissionAllMenu = this.hasPermission(this.$PermissioNames.pagesAdmin);
        this.items.push(
            { title: 'Página Inicial', icon: 'mdi mdi-home', link:'/app/administracao' },
            { title: 'Usuário', icon: 'mdi-account', link:'/app/usuario' },
            { title: 'Parceiro', icon: 'mdi-account-supervisor', link:'/app/parceiro' },
            { title: 'Projeto', icon: 'mdi-clipboard-text', link:'/app/projeto' }
        );
        if(this.hasPermissionAllMenu){
            this.items.push(
                { title: 'Concessionária', icon: 'mdi-city', link:'/app/concessionaria' },
                { title: 'Fase', icon: 'mdi-tune-vertical', link:'/app/fase' },
                { title: 'Tipo Anexo', icon: 'mdi-paperclip-plus', link:'/app/tipoAnexo' },
                { title: 'Tabela de Preço', icon: 'mdi-chart-timeline-variant-shimmer', link:'/app/tabelaPreco' },
                { title: 'Cobrança', icon: 'mdi-currency-usd', link:'/app/cobranca' },
            );
        }
    },

    watch: {

    },

    methods: {
        linkTo(link){
            this.$router.push(link);
            this.mini=true;
        },

        getParceiro(){
            this.Parceiro =
                this.$store.state.Session.tenant != null ? this.$store.state.Session.tenant.tenancyName : "Administrador"
        },

        getNomeSobrenome() {
            var nomeCompleto =
                this.$store.state.Session.user != null
                    ? this.$store.state.Session.user.name + " " + this.$store.state.Session.user.surname
                    : "";
            var arr = nomeCompleto.split(" ");
            if (arr) {
                this.Nome = arr[0];
                this.SobreNome = arr[arr.length - 1];
            }
        },


    }
}
</script>

<style scoped>

.linkItem{
    cursor: pointer;
}

.itensMenuLateral{
    font-family: 'Open Sans';
    font-style: normal;
    font-weight: 600;
    font-size: 16px;
    line-height: 20px;
    color: #4D4D4D;
    text-align: justify;
}
.linkItem {
    cursor: pointer;
}

</style>