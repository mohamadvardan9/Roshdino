/**
 * ServiceCard Component
 *
 * مسئول نمایش یک سرویس به صورت کارت.
 *
 * امکانات:
 * - دریافت اطلاعات سرویس از طریق Props
 * - نمایش آیکون، عنوان و توضیحات سرویس
 * - دارای انیمیشن Hover با Framer Motion
 *   (حرکت به سمت بالا و کمی بزرگ شدن کارت هنگام قرار گرفتن موس)
 * - شامل دکمه مشاهده بیشتر برای ورود به جزئیات سرویس
 *
 * @param service اطلاعات سرویس شامل:
 * - icon
 * - title
 * - description
 */



import type { Service } from "../data/services";
import { motion } from "framer-motion";
import Card from "./ui/Card";

interface Props {
  service: Service;
}

export default function ServiceCard({ service }: Props) {
  return (
    <motion.div
      whileHover={{
        y: -8,
        scale: 1.02,
      }}
    >
      <Card>
        <div className="text-5xl">{service.icon}</div>

        <h3 className="mt-6 text-xl font-bold">{service.title}</h3>

        <p className="mt-4 text-gray-500 leading-7">{service.description}</p>

        <button
          className="
        mt-6
        text-purple-600
        font-semibold
        hover:underline
        "
        >
          مشاهده بیشتر →
        </button>
      </Card>
    </motion.div>
  );
}
