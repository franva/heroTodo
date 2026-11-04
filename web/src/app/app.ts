import { Component, inject, signal, computed } from '@angular/core';
import { TodoService } from './services/todo.service';
import { Todo } from './models/todo';
import { TodoItemRow } from './components/todo-item-row/todo-item-row';

type Filter = 'all' | 'active' | 'completed';

@Component({
  imports: [TodoItemRow],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  private readonly todoService = inject(TodoService);
  private static readonly flag: Record<Filter, boolean | undefined> = {
    all: undefined,
    active: false,
    completed: true
  };
  
  protected readonly title = signal('Hero Todo App');
  protected readonly todos = signal<Todo[]>([]);
  protected readonly newTitle = signal('');
  protected readonly error = signal('');

  protected readonly openTodos = computed(() => this.todos().filter(todo => !todo.isCompleted));
  protected readonly completedTodos = computed(() => this.todos().filter(todo => todo.isCompleted));
  protected readonly remainingNum = computed(() => this.openTodos().length);
  protected readonly filters: Filter[] = ['all' , 'active', 'completed'];
  protected readonly filter = signal<Filter>('all');
  protected readonly currentFilter = signal<Filter>('all');
  
  ngOnInit() {
    this.load();
  }

  private load() {
    this.todoService.getAll(App.flag[this.filter()]).subscribe({
      next: (todos) => this.todos.set(todos),
      error: () => this.error.set("Loading failed. Check back later.")
    });
  }

  protected setFilter(f: Filter) {
    this.filter.set(f);
    this.load();
  }

  protected add(event: Event) {
    event.preventDefault();
    const title = this.newTitle().trim();
    if (!title) return;

    this.todoService.add(title).subscribe({
      next: (todo) => {
        this.todos.update(todos => [...todos, todo]);
        this.newTitle.set('');
        this.error.set('');
      },
      error: () => this.error.set("Failed to add todo. Check back later.")
    });
  }

  protected remove(todo: Todo) {
    this.todoService.remove(todo.id).subscribe({
      next:() => {
        this.todos.update(todos => todos.filter(i => i.id !== todo.id));
      },
      error: () => this.error.set("Failed to remove todo. Check back later.")
    });
  }

  protected update(todo: Todo, changes: Partial<Todo>) {
    this.todoService.update({...todo, ...changes}).subscribe({
      next: (updatedTodo) => {
        this.todos.update(todos => todos.map(i => i.id === updatedTodo.id ? updatedTodo : i));
        this.error.set('');
      },
      error: () => this.error.set("Failed to update todo. Check back later.")
    });
  }
}
