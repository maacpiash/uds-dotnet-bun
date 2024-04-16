<script lang="ts">
    import { enhance } from '$app/forms'
    export let data
</script>

<article class="flex flex-col md:flex-row justify-center gap-8">
    <form method="POST" action="?/create" use:enhance class="flex flex-col justify-center gap-4">
        <label>
            Task title
            <input name="title" type="text" placeholder="Title" class="input input-bordered w-full max-w-s" />
        </label>
        <div class="flex flex-row justify-start max-w-xs gap-4">
            <label for="done"> Done </label>
            <input id="done" name="done" type="checkbox" class="toggle" />
        </div>
        <button class="btn btn-primary">Submit</button>
    </form>

    <ul class="flex flex-col justify-center gap-8">
        {#each data.todos as todo}
            <li>
                <span>{todo.title}</span>
            </li>
        {/each}
    </ul>
</article>
