<template>
    <div class="flex-grow-1 d-flex flex-column">

        <h2 class="bbl-orange--text py-3">Administração</h2>
        
        <div class="rounded-lg pa-6 mx-6 mb-6 white elevation-2" style="max-width: 500px">
            <h3 class="text--bbl-neutral mb-4" style="color: #4C4C4C">Configurações do Sistema</h3>
            <hr class="mb-6" />
            <ul class="menu-list" style="padding: 0">
                <li>
                    <router-link to="/app/usuario" class="d-flex flex-row align-center">

                        <v-icon medium class="bbl-blue--text mr-3">mdi-account</v-icon>
                        <span style="color: #4C4C4C">Usuário</span>

                    </router-link>

                </li>
            </ul>
        </div>

        <div class="rounded-lg pa-6 mx-6 mb-6 white elevation-2" style="max-width: 500px" v-show="false">
            <h3 class="text--bbl-neutral mb-4" style="color: #4C4C4C">Relatórios</h3>
            <hr class="mb-6" />
            <ul class="menu-list" style="padding: 0">
                <li>
                    <router-link to="/app/usuario" class="d-flex flex-row align-center">
                        <v-icon medium class="bbl-blue--text mr-3">mdi mdi-file-document-check</v-icon>
                        <span style="color: #4C4C4C">Projetos Aprovados</span>
                    </router-link>
                </li>
                <li>
                    <router-link to="/app/usuario" class="d-flex flex-row align-center">
                        <v-icon medium class="bbl-blue--text mr-3">mdi mdi-file-table</v-icon>
                        <span style="color: #4C4C4C">Cobranças Emitidas</span>
                    </router-link>
                </li>
                <li>
                    <router-link to="/app/usuario" class="d-flex flex-row align-center">
                        <v-icon medium class="bbl-blue--text mr-3">mdi mdi-text-box-search</v-icon>
                        <span style="color: #4C4C4C">Verificar Requisito e adicionar o outro relatório</span>
                    </router-link>
                </li>
            </ul>
        </div>

    </div>


</template>

<script>
export default {
  name: 'ViewAdministracao'
}
</script>

<style scoped>

a {
    text-decoration: none !important;
}

.menu-list li {
    list-style: none;
    display: flex;
    flex-direction: row;
    align-items: center;
    font-weight: 400;
    color: #434343;
    font-size: 16px;
    margin: 16px 0;
    cursor: pointer;
}

.menu-list li:hover {
    font-weight: 500;
}
</style>