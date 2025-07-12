<template>
  <div v-if="message" class="alert alert-danger text-center py-2">
    {{ message }}
  </div>
</template>

<script>
export default {
  name: 'ErrorMessage',
  props: {
    message: { type: String, default: '' }
  }
};
</script>