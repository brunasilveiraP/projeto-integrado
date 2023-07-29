<template>
  <div class="flex-grow-1">
   <div class="d-flex align-center">
    <h2 v-if="title" class="bbl-blue--text pt-3 pb-5 pr-3">{{ title }}</h2>
    <slot name="title-custon"></slot>
   </div>
  </div>
</template>

<script>
export default {
    components: {  },
    props: {
        title: {type: String , default: null},
    },
    data() {
      return {
      }
    },
    methods: {
    }
}
</script>

<style lang="scss" escoped>
    
</style>