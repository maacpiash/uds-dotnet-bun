<script lang="ts">
    import { enhance } from '$app/forms'
    export let data
</script>

<article class="flex flex-col md:flex-row justify-center gap-8">
    <form method="POST" action="?/create" use:enhance class="flex flex-col justify-center gap-4 max-w-xs">
        <label class="text-xl font-semibold">
            Add new task
            <input
                name="title"
                type="text"
                placeholder="Title"
                class="input input-bordered input-primary w-full mt-3" />
        </label>
        <label class="cursor-pointer label">
            <span class="label-text">Done</span>
            <input type="checkbox" class="toggle toggle-primary" checked name="done" />
        </label>
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
