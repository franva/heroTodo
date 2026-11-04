import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TodoItemRow } from './todo-item-row';

describe('TodoItemRow', () => {
  let component: TodoItemRow;
  let fixture: ComponentFixture<TodoItemRow>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TodoItemRow],
    }).compileComponents();

    fixture = TestBed.createComponent(TodoItemRow);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
