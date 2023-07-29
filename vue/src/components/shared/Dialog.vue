<template>
    <v-dialog
        v-model="showDialog"
        persistent
        max-width="1000px"
        width="1000px"
    >
        <v-card>

            <v-card-title class="justify-center text-h5 lighten-4">
                <v-container fluid pa-0>
                    <v-row justify="space-between" class="pa-2 mx-0 mt-1">
                        <v-col>
                            <v-row>
                                <h2 class="bbl-neutral--text mb-2">{{!editId ? title : titleEdit }}</h2>
                                <v-btn v-if="editId && isUsuario"
                                       color="grey"
                                       class="white--text hover-click ml-4"
                                       @click="$emit('showModalRecuperacao', true)"
                                >
                                    Redefinir Senha
                                    <v-icon  class="rotate-icon" right dark>mdi-lock</v-icon>
                                </v-btn>
                            </v-row>
                        </v-col>

                        <v-col align="start">
                            <v-row justify="end">
                                <v-btn text small fab @click="(() => showDialog = false)" class="hover-click">
                                    <v-icon color="grey">mdi-close</v-icon>
                                </v-btn>
                            </v-row>
                        </v-col>
                    </v-row>

                    <v-row class="px-4">
                        <v-divider></v-divider>
                    </v-row>
                </v-container>
            </v-card-title>

            <v-container fluid class="pl-4">
            <slot></slot>
            </v-container>

            <v-card-actions class="pb-0">
                <v-row class="mb-0 pr-4" justify="end">
                    <v-col align="end">
                        <v-btn
                            class="bbl-orange white--text hover-click"
                            @click="$emit('submit')"
                            :loading="loading"
                        >
                            Salvar
                        </v-btn>
                    </v-col>
                </v-row>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<script lang="ts">

export default {
    name: 'alert-dialog',
    props: {
        value:  {type: Boolean, default: false},
        loading:  {type: Boolean, default: false},
        editId:  {type: Number, default: null},
        title: {type: String, default: ""},
        titleEdit: {type: String, default: ""},
        isUsuario : {type: Boolean, default: false}
    },
    data: () => ({

    }),
    computed: {
        showDialog: {
            get() {
                return this.value;
            },
            set(newValue) {
                this.$emit("input", newValue);
            }
        }
    },
}
</script>

<style lang="scss" scoped>
.v-card__text {
    color: var(--v-black-base) !important;
}
</style>
