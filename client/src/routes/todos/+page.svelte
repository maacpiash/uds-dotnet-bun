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

    <ul class="flex flex-col justify-center gap-1">
        {#each data.todos as todo}
            <li>
                <form class="flex flex-row place-content-center gap-2" method="POST" action="?/toggle" use:enhance>
                    <input type="hidden" name="id" value={todo.id} />
                    <label class="cursor-pointer label flex flex-row gap-2">
                        <span class="label-text text-xl">{todo.title}</span><button
                            class="btn btn-sm btn-warning btn-circle btn-outline"
                            ><svg
                                xmlns="http://www.w3.org/2000/svg"
                                class="h-6 w-6"
                                fill="none"
                                viewBox="0 0 24 24"
                                stroke="currentColor"
                                ><path
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                    stroke-width="2"
                                    d="M6 18L18 6M6 6l12 12" /></svg
                            ></button>
                    </label>
                </form>
            </li>
        {/each}
    </ul>
</article>
