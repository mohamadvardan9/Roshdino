export interface Service {
  id: number;
  title: string;
  description: string;
  icon: string;
}

export const services: Service[] = [
  {
    id: 1,
    title: "طراحی سایت",
    description: "طراحی وب‌سایت‌های مدرن، سریع و واکنش‌گرا.",
    icon: "🌐",
  },
  // {
  //   id: 2,
  //   title: "سئو",
  //   description: "بهینه‌سازی سایت برای موتورهای جستجو.",
  //   icon: "📈",
  // },
  {
    id: 2,
    title: "تولید محتوا",
    description: "تولید محتوای هدفمند برای رشد برند.",
    icon: "📸",
  },
  {
    id: 3,
    title: "برندینگ",
    description: "ساخت هویت بصری و برند حرفه‌ای.",
    icon: "✨",
  },
  {
    id: 4,
    title: "تبلیغات دیجیتال",
    description: "اجرای کمپین‌های تبلیغاتی پربازده.",
    icon: "🚀",
  },
  // {
  //   id: 6,
  //   title: "هوش مصنوعی",
  //   description: "پیاده‌سازی راهکارهای AI برای کسب‌وکارها.",
  //   icon: "🤖",
  // },
];
